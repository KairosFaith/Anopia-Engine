# Anopia Engine
This is a system created to make it easier for you to implement sound effects and music in Unity.
There are no external code dependencies, as it only uses the native audio functions in unity.
Functions such as Audio Filters or pitch and volume randomisation can be set up very easily with this system.
## Setting up
Anopia Engine uses scriptable objects (called Mags) to hold reference data for your audio clips.
Each type of sound effect or music implementation has a corresponding scriptable object.
## Sound Effects
### ADSR
### Transient Sounds
#### ClipMag/OneShotEvent
This Event uses the 'AudioSource.PlayOneShot' function on a persistent audio source.
#### ClipObjectMag
## Dynamic Music System
### Stem Music Transitions
### Linear Music Transitions
### Synchro Events
