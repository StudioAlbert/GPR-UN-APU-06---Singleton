# GPR-UN-APU-06 — Singleton en Unity

Companion for the session *Singleton en Unity* (SAE Institute Geneva, GP-926).

A deliberately tiny demo — one `AudioManager`, one `ScoreKeeper`, no gameplay to speak of. It
exists to be **played in front of the class and broken on purpose**: every chapter of the deck
is one failure of the singleton, and its answer.

| | Unity 6000.4.10f1 · URP 2D |
|---|---|
| Branch | **one**, `main`. A chapter is a folder of scenes and a namespace, not a branch |
| UI | IMGUI (`OnGUI`), on purpose: the demo is about the managers, not the interface |
| Rebuild the scenes | menu *Companion › Rebuild the demo scenes* |

```bash
git clone https://github.com/StudioAlbert/GPR-UN-APU-06---Singleton.git
```

Every clip played also **flashes the screen**. The flash is not the service, it is the
confirmation that the call reached someone: a classroom projector has no speakers, and the back
of the room has to see something happen. When nothing flashes, nothing was played.

## Four chapters, one project

| Chapter | Scenes | Namespace | What it adds |
|---|---|---|---|
| 1 | `01 - Basic` | `Chapter1Basic` | a static field and a check in `Awake` |
| 2 | `02 - Generic` | `Chapter2Generic` | `Singleton<T>`, and `RegulatorSingleton<T>` to swap in |
| 3 | `03 - Null object` | `Chapter3NullObject` | `IAudioService`, `SilentAudio`, `Sound` |
| 4 | `04 - Persistent` | `Chapter4Persistent` | `PersistentSingleton<T>`, and the null object that logs |

The scripts of a chapter live in `Assets/03 - Scripts/<chapter>/` and nowhere else; its scenes
only ever reference its own components. So the four chapters sit side by side in the same
checkout, and moving on in the deck means opening another scene — no `git switch` in the middle
of a lecture.

Three folders are shared: `Singletons/` (the three base classes, the only code the session
actually teaches), `Shared/` (the flash, the IMGUI style, the bootstrap loader) and
`05 - Audio/`.

## Four scenes per chapter

- **`NN - Level`** — the level, with a `Managers` object of its own (`AudioManager` +
  `ScoreKeeper`), exactly the way everyone writes them first. *Shoot* (a clip, a flash and ten
  points), *Next level*, *Load the HUD scene on top*, *Reset the score*.
- **`NN - Level 2`** — the same level, **without the managers**. It is the next level nobody
  equipped, and it is where the session starts.
- **`NN - HUD`** — a real HUD: it prints the score in the top-right corner, and it brings a
  second `Managers` object along. Loaded additively, it is the duplicate, and the score it
  prints tells you which manager answered.
- **`NN - Bootstrap`** — one `Managers` object created before the game, then `NN - Level`. It
  buys nothing until chapter 4: before that, it dies with its own scene.

Opening a level and pressing Play, without going through any bootstrap, is the normal way to use
this project. It is what students do every day, and it is what breaks.

### Chapter 1 — the three failures

1. Open `01 - Level 2`, press Play, click *Shoot* → `NullReferenceException`, and the stack
   blames `LevelHud`, which did nothing wrong.
2. Open `01 - Level`, press Play, shoot — it works — then *Next level* → no more sound, no
   more flash, and the score is back to zero. Both managers went with the scene they lived in.
3. From `01 - Level`, click *Load the HUD scene on top* → the HUD appears in the corner with
   the right score, but a second `Managers` came with it, and the newcomer is the one that
   dies. Watch it vanish from the Hierarchy in the same frame it arrived.

The score line in `LevelHud` carries the one null test of the whole project. Without it the HUD
throws sixty times a second and you never get as far as clicking anything. It is exactly the
defensive code chapter 3 deletes.

### Chapter 2 — the plumbing, written once

`AudioManager` and `ScoreKeeper` lose their `Awake` boilerplate: `Singleton<T>` carries it. The
accessor builds an instance when none is there, so failure 1 is gone — not fixed, *hidden*.
Open `02 - Level 2` and press Play: a `ScoreKeeper Auto-Generated` is **already in the
Hierarchy**, before anyone clicks, because the HUD reads the score every frame. Click *Shoot*
and an `AudioManager Auto-Generated` joins it. They work, they carry whatever the code happens
to default to, the manager has no clip to play — and nobody placed them or can configure them.
They also belong to the current scene, so *Next level* throws them away and the next click
builds new ones. The crash of chapter 1 has become a score that silently restarts at zero.

Failure 3 is still there, and worse: shoot a few times, then load the HUD scene on top of
`02 - Level`. Both score lines drop to **zero** — the level's panel included — while the points
are still held by the first `ScoreKeeper`, which nothing reads any more. Count the managers:
there are **two**, both alive, and `Instance` is whichever woke up last.
`Singleton<T>` never destroys anything. Change `AudioManager : Singleton<AudioManager>` to
`RegulatorSingleton<AudioManager>`, load the HUD scene again, and watch the console: now the
**older** instance is destroyed. The opposite policy from chapter 1, and neither one is wrong —
what is wrong is not knowing which one you wrote.

### Chapter 3 — never `null` again

`Sound.Current` always answers. With no manager around, it is a `SilentAudio` that honours
nothing. Open `03 - Level 2`, press Play, click *Shoot*: the score goes up, the screen stays
flat, the room stays quiet, no error. Then open `03 - Level`: same code, sound and flash
included.

The price is on that first run: nothing tells you the service is missing.

The null object answers for the audio, not for the rest: load the HUD scene on top of
`03 - Level` and the score still drops to zero. `ScoreKeeper` is a plain singleton here, and
the duplicate bites it exactly as before — an access that never returns `null` is not an
answer to two instances.

### Chapter 4 — surviving the scenes

`PersistentSingleton<T>` unparents the object and calls `DontDestroyOnLoad`. Open `04 - Level`,
shoot a few times, then *Next level*: in `04 - Level 2`, which carries no manager at all, the
sound still answers and the score is still there. In the earlier chapters, both are gone.

It is also the first chapter where the bootstrap means something: `04 - Bootstrap` creates the
managers, `04 - Level` brings its own, and the newcomer is destroyed — one instance, whichever
scene you started from.

Its `SilentAudio` is the one difference left: it logs a warning. The game still never crashes,
and a missing service leaves a trace. That is the whole fix.

> **The ten-second demo of the first trap**: in `04 - Level`, drag `Managers` under any other
> object and press Play. `DontDestroyOnLoad` only moves a **root** object — except
> `autoUnparentOnAwake` is checked, so it still works. Uncheck it, play again and hit *Next
> level*: the manager dies with its scene. Unity did say so — *DontDestroyOnLoad only works for
> root GameObjects* — but as a warning, which nothing forces you to read.

## Why the score is in here

`ScoreKeeper` is a singleton, like the audio service — and it should not be. A score belongs to
a run, not to the game. In chapter 4, shoot a few times and walk the two levels back and forth:
the score follows you everywhere, with no way to start a new run. It is the bug the deck warns
about, left in on purpose.

The *Reset the score* button is the workaround everybody writes instead of fixing the design.

## Credits

`Singleton<T>`, `PersistentSingleton<T>` and `RegulatorSingleton<T>` follow
[adammyhre/Unity-Utils](https://github.com/adammyhre/Unity-Utils). The null object follows the
*Service Locator* chapter of [Game Programming Patterns](https://gameprogrammingpatterns.com/service-locator.html).
