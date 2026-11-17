# GPR-UN-APU-06 — Singleton en Unity

Companion for the session *Singleton en Unity* (SAE Institute Geneva, GP-926).

A deliberately tiny demo — three scenes, two managers, no gameplay to speak of. It exists to
be **played in front of the class and broken on purpose**: every branch is one chapter of the
deck, and every chapter is one failure of the singleton and its answer.

| | Unity 6000.4.10f1 · URP 2D |
|---|---|
| Scenes | `Assets/01 - Scenes/` — `Bootstrap`, `Menu`, `Level`, `HUD` |
| UI | IMGUI (`OnGUI`), on purpose: the demo is about the managers, not the interface |
| Rebuild the scenes | menu *Companion › Rebuild the demo scenes* |

## The four scenes

- **`Bootstrap`** — the only place that creates the services. Holds `Managers`
  (`AudioManager` + `ScoreKeeper`) and loads `Menu`.
- **`Menu`** — the score so far, *Play*, and a button to reset the score.
- **`Level`** — *Shoot* (a sound and ten points), *Back to the menu*, and a button that loads
  `HUD` **on top** of the current scene.
- **`HUD`** — nothing but a second `Managers` object. It is the duplicate.

Playing `Level` directly, without going through `Bootstrap`, is the normal way to use this
project. It is what students do every day, and it is what breaks.

## One branch per chapter

Each branch is the same project with a different manager. **The scenes never change** — only
the scripts under `Assets/03 - Scripts/`.

| Branch | What it adds | What to show |
|---|---|---|
| `01-basique` | a static field and a check in `Awake` | the three failures, in three clicks |
| `02-generique` | `Singleton<T>`, `RegulatorSingleton<T>` | the plumbing written once; the duplicate settled by age |
| `03-nullobject` | `IAudioService`, `SilentAudio`, `Sound` | `Level` plays on its own, silently, without one error |
| `04-persistent` | `PersistentSingleton<T>` | the music no longer restarts between scenes |
| `main` | the null object logs | the silent failure made visible again |

```bash
git clone https://github.com/StudioAlbert/GPR-UN-APU-06---Singleton.git
cd GPR-UN-APU-06---Singleton
git switch 01-basique
```

### `01-basique` — the three failures

1. Open `Level`, press Play, click *Shoot* → `NullReferenceException`, and the stack blames
   `LevelHud`, which did nothing wrong.
2. Open `Bootstrap`, press Play, click *Play* → the music stops. The manager went with the
   menu scene.
3. From `Level`, click *Load the HUD scene on top* → a second `AudioManager` arrives, and the
   newcomer is the one that dies.

The score line in `LevelHud` carries the one null test of the whole project. Without it the HUD
throws sixty times a second and you never get as far as clicking anything. It is exactly the
defensive code chapter 3 deletes.

### `02-generique` — the plumbing, written once

`AudioManager` and `ScoreKeeper` are four lines each now. The accessor builds an instance when
none is there, so failure 1 is gone — not fixed, *hidden*: the auto-generated manager has no
clips, so the game runs silent and says nothing.

Change `AudioManager : Singleton<AudioManager>` to `RegulatorSingleton<AudioManager>`, load
the HUD scene on top again, and watch the console: the **older** instance is destroyed. The
opposite policy from branch `01`, and neither one is wrong — what is wrong is not knowing
which one you wrote.

### `03-nullobject` — never `null` again

`Sound.Current` always answers. With no manager around, it is a `SilentAudio` that honours
nothing. Open `Level` on its own, press Play, click *Shoot*: the score goes up, no sound, no
error. Then run from `Bootstrap`: same code, sound included.

The price is on the last click: nothing tells you the audio is missing.

### `04-persistent` — surviving the scenes

`PersistentSingleton<T>` unparents the object and calls `DontDestroyOnLoad`. The music now
runs from `Bootstrap` to `Menu` to `Level` without a hiccup, and the score survives with it.

> **The ten-second demo of the first trap**: in `Bootstrap`, drag `Managers` under any other
> object and press Play. `DontDestroyOnLoad` only works on a root object — except
> `autoUnparentOnAwake` is checked, so it still works. Uncheck it, play again: the music stops
> at the first scene change, and nothing in the console says why.

### `main` — the null object that tells on itself

One file apart from `04-persistent`: `SilentAudio` now logs a warning. The game still never
crashes, and a missing service leaves a trace. That is the whole fix.

## Why the score is in here

`ScoreKeeper` is persistent, like the audio service — and it should not be. A score belongs to
a run, not to the game. Play, score a few points, go back to the menu, start again: the score
is still there. It is the bug the deck warns about, left in on purpose.

The *Reset the score* button in the menu is the workaround everybody writes instead of fixing
the design.

## Credits

`Singleton<T>`, `PersistentSingleton<T>` and `RegulatorSingleton<T>` follow
[adammyhre/Unity-Utils](https://github.com/adammyhre/Unity-Utils). The null object follows the
*Service Locator* chapter of [Game Programming Patterns](https://gameprogrammingpatterns.com/service-locator.html).
The two audio clips are generated, not sampled.
