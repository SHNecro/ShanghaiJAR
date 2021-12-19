######################################################
ShanghaiEXE Genso Network Map Editor 0550e1 (12/19/2021)
######################################################

-------- Changelog --------
---- 0.550e1 ----
Moved some constant data out to external files so map editor can edit (data/data/*.xml)
Added editors for external data
Added music player, audio engine to map editor for bgm/sound effects
---- 0.503e3 (12/21/2019) ----
Fixed NormalNavi chip corruption
---- 0.503e1 (12/19/2019) ----
Added various effects used by new cutscenes
---- 0.502e16 (11/09/2019) ----
Fixed adding of default untranslated string keys
Changed default new page event to invisible with no objects
---- 0.502e14 (9/28/2019) ----
Added Settings page for paths and graphics file options
Added TCD pack/unpack utility
Added loading screen for long loading time
Added on-map id labels
Added selection link for targeted ids in EventMove, etc.
Added zooming on map view
Added text/face preview in text editor
---- 0.502e13m3 (07/25/2019) ----
Various bugfixes
Added unsaved change tracking/confirmation
Added error handling for opened files
---- 0.502e13m1 (formerly 1.0) (07/11/2019) ----
Initial Release

--- Planned (unimplemented) Features ---
Dialogue events editor (pauses, phonecall, etc.)
Effects preview
Full event/cutscene preview
Better map screen controls (drag and select multiple, scroll to focused/doubleclicked object, etc.)
Controls to hide/show objects based on flags, etc.
Documentation of flags and variables

-------- Shortcuts --------
--- Main Map Screen ---
	Scrollwheel:
		No modifiers: Up/Down
		Ctrl+Shift: Zoom in/out
		Shift: Left/Right
		Ctrl (Hovering over object): Change to next overlapping hovered object
		  Clicking will select the hovered object
		Ctrl (Drawing): Move cursor up/down a level
	Arrow Keys (Item selected):
		No modifiers: Move object along map coordinates
		Shift: Move x8
		Alt: Move object directly up/down/left/right
	Delete (Item selected):
		No modifiers: Delete
		Shift: Delete without confirmation
	Ctrl + D (Item selected):
		Duplicate
--- Editors ---
	Lists:
		Arrow Keys (Item selected):
			Alt: Move object up/down in list
		Delete (Item selected):
			No modifiers: Delete
			Shift: Delete without confirmation
		Ctrl + D (Item selected):
			Duplicate
	String Editor
		PageUp:
			Go to previous page
		PageDown:
			Go to next page
		Ctrl + N:
			New string entry
		Ctrl + O (Item selected):
			Edit selected entry
		Delete (Item selected):
			No modifiers: Delete
			Shift: Delete without confirmation