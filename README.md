# ABOUT

This prototype was part of the work done for an internship at NotAGameStudios for a Digital Games Development course that I took. Just like the other project, this was supposed to be a collaborative work, but due to time constraints for the other intern, only I touched this project. As such, all work on this project (with the exception of the music track) was done by me.

This project is a 2D multiplayer rhythm game with a level editor (for developers), responsive elements to input and accuracy, made in Unity Engine. Most of the work in this project lies in the programming. First use of #region to organize and collapse code.

# GAMEPLAY FLOW
- Scene begins with a countdown to give the player time to prepare
- After countdown, music track starts playing and notes start to move
- Hit the respective keys when the note is near the hit markers to score.
- Background elements react or activate depending which player scores
- If a player misses, combo is reset
- NOTE: The game does not end due to limited time to work on the project *

# CREATED ASSETS
- Technical document (A document to explain the project, such as folder structure, planned mechanics, scene arrangement, etc.)
- GIT setup using a client (gitignore file, repo creation, invited other developers)
- Visual elements made in GIMP
  - Note outlines
  - Backgrounds
  - Background "combo" objects
- Programming and Scripts
  
# MECHANICS
- Level editor for developers. Can insert notes, delete notes, jump to a certain point in the music track, play/stop music, change playback speed and save the note chart into a binary file *
- Note scroller. Several calculations to make sure everything is in sync with the note times
- Simple UI. Shows time elapsed and stats for both players, such as score, combo and details about the last note that was pressed or missed
- Distance calculations between notes and hit markers. Can measure key press accuracy
- Load data from a binary file, more specifically, the files created from the level editor
- Background elements reacting according to which player is correctly hitting the notes

*Mechanics marked with * are unfinished to some extent due to limited time to refine or fully complete said systems
