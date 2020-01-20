###############################################
ShanghaiEXE Genso Network 0.503e4 (1/4/2020)
###############################################

-------- Installation --------
The game may require the following to be installed:

DirectX End-User Runtime
https://www.microsoft.com/ja-jp/download/details.aspx?id=35
The game uses DirectX 9, which does not come installed by default
on modern computers (Vista onwards)

Microsoft .NET Framework 4
https://www.microsoft.com/ja-jp/download/details.aspx?id=17851

Microsoft .NET Framework 4.5
https://www.microsoft.com/ja-jp/download/details.aspx?id=30653
The .NET Frameworks may already be installed on modern computers.

SlimDX　January 2012　End User Runtime
https://code.google.com/archive/p/slimdx/downloads
This may not be needed, as a fresh image was able to run without
installing this.

-------- Information --------
The project was previously hosted at github, but that got DMCAd so
there's no set place to report bugs/get the latest release.
If a suitable host is found, hopefully I'll remember to update this note.
The commit history may not be up to date, as I use a personal repo
for source control and push to the public repo in batches.

-------- Compatibility with previous versions (<0.502e) --------
---- Savegames ----
save.she should be fully compatible with this version,
and saves from this version should be usable with older ones, if
you decide to do that for whatever reason (minus new chips, etc.).

---- Config Files ----
option.cfg has been replaced with a more readable
xml file. The game and KeyConfig application can convert a .cfg
into a .xml, with different behavior depending on if .xml is already
present.
-- .cfg and .xml --
The game will take the .xml and rename the .cfg to .cfg.OLD
KeyConfig will prompt you to overwrite the .xml with the .cfg's data,
and load the .cfg data to edit (unless X is pressed)
-- only .cfg --
The game will load the .cfg, rename it to .cfg.OLD, and create a .xml
KeyConfig will load the .cfg data to edit, but will not create a .xml
until "Save Changes" is pressed
-- only .xml --
The .xml will be used

---- Data files ----
The map data files (.she) are not compatible at all, and will crash
the game if an invalid one is present.
The language files are new, and will obviously not work with old versions.

-------- Known Issues --------
The sound stops working when plugging in or unplugging headphone/speakers:
I haven't looked too deeply at the audio parts of the code, so I never fixed it.
Just don't unplug your audio, or exit and reopen the game.

The screen is blurry for scaled-up sizes:
Update: Implemented a OpenGL engine which should solve blur issues.
I recommend using OpenGL since I actually understand that engine and
can maintain it.

The config file codes don't match any standard keycodes:
That's how the game reads keys, and I'm not rewriting the input
section/breaking backwards compatibility just for a minor config file issue.

Some controller buttons aren't registering:
Update: Added XInput support, controllers should be working now.


-------- Unknown Issues (please report) --------
Missing content:
I may or may not continue, but it would be very helpful if someone
could compile what's missing, since I haven't watched any dev streams
or teased content.
Transcribed Japanese text or even translations would be nice, too.
Same with stitched area maps, sprites, etc. from streams.

Text alignment and mistranslations:
If text doesn't fit onscreen/is badly aligned, that can be fixed.
Or you can fix it yourself (in the language files), and share.
Likewise for bad translations, I don't even speak Japanese.

Other bugs:
Introducing the Turbo button required breaking apart the rendering
and game tick loops, which is how it looks like the game was designed
in the first place but the two got tangled up and hacked together.
Other hacks got introduced to deal with that, and those may have broken.

-------- Changelog --------
---- 0.503e4 (1/4/2020) ----
Fixed Yuyuko crash on heavy+sand
Fixed Hospital event BGM override
Fixed shaking if skipping during cutscene
Changed object cleanup from Render to Update loops, fixing possible issues w/ turbo
  Known fixed: Duplicated effects like bullet shells and shadow steps
Modified logos/labels to distance from original creator
Re-added chip icons for new illegal chip ShellHoc
---- 0.503e3 (12/21/2019) ----
Fixed graphics jitter/off-by-1 glitches
(Probably) fixed or severely reduced black boxes in text
Adjusted ending+credits BGM based on feedback
HOTFIX _1 (12/22/2019): Fixed demo-end sprite being visible before demo end
HOTFIX _2/3 (12/28/2019): Fixed postgame Wriggle showing up too early
                        : Fixed OpenGL fullscreen, added aspect ratio option
---- 0.503e2 (12/20/2019) ----
Fixed critical bug where NormalNavi fights did not use any chips
Added game-complete star
Fixed Engelles BGM loop
Fixed MimaDS chip crash
Fixed eien npc floor position
---- 0.503e1 (12/19/2019) ----
First new story content since cancellation on 12/19/2018
  Final dungeon boss cutscene
  Final dungeon boss battle (V1)
    Final boss dungeon V1 chip
  Final boss cutscene
    Final boss battle (already existed)
  Ending cutscene
  Credits
  Epilogue Start
    Epilogue world NPCs, end-of-content gate
---- 0.502e.16 (11/08/2019) ----
Integrated anon's ShanghaiDS, Flandre, and Yuyuko fights into debug room
Bugfixes:
  Occasional doubled input (when catchup updates occurred on initial keydown)
  Japanese mysterydata text printed "battlechip" in place of name, and no name
  Events double-fired if multiple updates happened before a render, causing camera issues
  Removed leftover ROM navi events after comp cleared
---- 0.502e.15 (10/29/2019) ----
Reworked OpenGL text rendering with per-glyph rendering, fixing jumpy text
  Found default fonts, fallback fonts, adjusted sizes to match original
Implemented XInput controller support
---- 0.502e.14 (9/28/2019) ----
Added OpenGL as a rendering option, fixing blurriness issues
  Replaces DirectX9 as default since I can maintain it now
  Minor differences in how text is rendered, possibility of overflow
  DirectX9 is still available as an option in the KeyConfig application
Fixed previous half-assed implementation of Turbo, better FPS display
Minor randomly noticed text fixes
---- 0.502e.13 (03/29/2019) ----
Various text fixes after actually seeing the scenes they were used in
---- 0.502e.12 (03/20/2019) ----
Fixed virus manager HELP crash for viruses with V code
Added missing virusball portraits as yellow filter over existing chips
Added handling for same-frame mutual-kill
Fixed Shrine heat event button disable sticking if warping mid-fire
Add option (default on) to fix original english errors, to be added to
---- 0.502e.11 (03/19/2019) ----
Added check in game for allowing slowdown
Fixed SELECT behavior in folder edit to do nothing on empty chip
Fixed BGM override on scripted battles (now restore pre-battle BGM)
Fixed land plane typo
Added ability to disable incident/emergency BGM override in config
---- 0.502e.10 (03/19/2019) ----
Fixed addon manager exit option text
Added Turbo button to modify the game updates per second
Fixed title FPS calculation
Added ability to set display FPS and Turbo update speed in config
Added opt-in option in Extras to allow setting lower "turbo" cap
---- 0.502e.9 (03/17/2019) ----
Fixed Sakuya knives spawning outside battle area
Fixed missing language keys in japanese language files
---- 0.502e.8 (03/17/2019) ----
Numerous text fixes (thanks anon)
Translated / found out what new AddOns do / fixed IDs
---- 0.502e.7 (03/16/2019) ----
Probably(?) fixed render order for intangible objects
Fixed one Eien railing with incorrect render order
Fixed typo (not mine) in Wriggle text on cruiseship
Fixed chipreader off-by-one error on owned/unowned chips
---- 0.502e.6 (03/16/2019) ----
Fixed face mixup for female navi in Eien 3 (Chapter 4)
Fixed normal chip trader text to 3 chips.
---- 0.502e.5 (03/15/2019) ----
Added special handling for period pausing to handle "Dr.", "Ms.", "Mr."
---- 0.502e.4 (03/14/2019) ----
Translated names of random encounter enemies with names
---- 0.502e.3 (03/13/2019) ----
Fixed Request BBS showing "cannot take this request" in all cases
---- 0.502e.2 (03/12/2019) ----
Changed Paused When Inactive to actually pause the game rather than just inputs
---- 0.502e.1 (03/10/2019) ----
Fixed crash involving interiors looking for pre-translation strings
---- 0.502e (03/09/2019) ----
Added translations for content added since last public release (0.43)
Reworked translation system. (requiring changes to map files)
Changed config system for new options
Added new options to config / new config program:
Dialogue tester, to show xml dialogues to the game without
reopening/rebuilding the game. Left it there for text corrections.
Lowering of busting rank, to farm specific chips without having to
sit around for 10 seconds/get hit to deliberately lower the rank.
Added givememoney.rtf

---- (12/19/2018) ----
The game gets shut down by Capcom.
---- (09/04/2018) ----
The original developer opens and puts put builds behind a ci-en/patreon paywall.
---- (earlier) ----
Earlier versions / changelogs can be found at:
http://www.geocities.jp/mahodenn/thlal.html
until March 19, 2019 when Geocities dies (it's dead).