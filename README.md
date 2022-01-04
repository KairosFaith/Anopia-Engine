# Readme under construction XD
# Setting up
Anopia Engine uses scriptable objects (called Mags) to hold reference data for your audio clips.
Each type of sound effect or music implementation has a corresponding scriptable object.
## Driver
Manually Add `anDriver` as a component to your game object.
In your gameplay script, call the `SetDriver(MonoBehaviour host, AudioMixerGroup output, params string[] IDs)` Function to assign the driver host, mixer output, and Load audio events using the names of the corresponding scriptable objects.
To start and stop events using the driver, simply activate `anDriver.Play(string SoundID, params object[] args)` and `anDriver.Stop(string SoundID)`. The SoundID will be the name of the scriptable object used to Load the event.
## Events

## Sound Effects
### ADSR
### Transient Sounds
#### ClipMag/OneShotEvent
This Event uses the [AudioSource.PlayOneShot](https://docs.unity3d.com/ScriptReference/AudioSource.PlayOneShot.html) function on a persistent audio source.
#### ClipObjectMag
## Dynamic Music System
### Stem Music Transitions
### Linear Music Transitions
### Synchro Events
