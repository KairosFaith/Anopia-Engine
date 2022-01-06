# Readme under construction XD
# Setting up
Anopia Engine uses scriptable objects (called Mags) to hold reference data for your audio clips.
Each type of sound effect or music implementation has a corresponding scriptable object.<br/>
1. Decide what implementation you need and find the corresponding Mag to use
2. Create the scriptable object in your unity assets.
3. Assign the audio clips, sound object prefabs and any other settings to the scriptable object.
## Sourcerer
Any Audio Source Game Object that is instantiated by the system will use the `anSourcerer` MonoBehaviour Class.
Some scriptable objects will require you to set up prefabs that hold the AudioSource Component. Settings will need to be adjusted on the prefab component as well. (see ClipObjectMag/ADSRMag/LayerMag below)
## Driver
### In Editor
Manually Add `anDriver` as a component to your game object.
If you are using a `OneShotEvent`(see below), you will need to manually create and assign an audio source too. This Audio Source will be used to play all `OneShotEvent`s assigned to the driver.
### In Runtime
In your gameplay script, call the `SetDriver(MonoBehaviour host, AudioMixerGroup output, params string[] IDs)` Function to assign the driver host, mixer output, and Load audio events using the names of the corresponding scriptable objects.
### Play and Stop Sounds
To start and stop events using the driver, simply activate `anDriver.Play(string SoundID, params object[] args)` and `anDriver.Stop(string SoundID)`. The SoundID will be the name of the scriptable object used to Load the event.
# Driver Events
`IanEvent`s are non MonoBehaviour classes used with the `anDriver` for your convenience, but they can be used on their own as well.
### Transient Sound Events
#### ClipMag/OneShotEvent
* Randomly selects an audio clip, randomises the volume and plays it using the attached AudioSource. See [AudioSource.PlayOneShot](https://docs.unity3d.com/ScriptReference/AudioSource.PlayOneShot.html)

<br/>Each `ClipData` set has an audioClip reference with its own gain adjustment. `VolumeFluctuation` controls the range of randomisation that occurs with the volume.
Use `Play` to play sound. `args[0]` as float is used for volume scale, gain and randomisation still applies. `args[1]` as int will be used as an array key to access the audio clip.
`Stop` will activate the Stop function on the audio source.
#### ClipObjectMag/ClipObjectEvent
* Randomly selects an audio clip, randomises the volume and plays it with an instantiated audioSource for each transient.
* You can play sounds at any position and at any timecode.
* You also have pitch, HighPass and Distortion randomisation

<br/>Use `Play` to play sound and `PlayScheduled` to play sound at a specified timecode.
`args[0]` as float is used for volume scale, gain and randomisation still applies. `args[1]` as Vector3 is the position where the sound object is instantiated, the position will be the host object position if unspecified.
If you are using HighPass and Distortion randomisation, you MUST add the [AudioHighPassFilter](https://docs.unity3d.com/ScriptReference/AudioHighPassFilter.html) and [AudioDistortionFilter](https://docs.unity3d.com/ScriptReference/AudioDistortionFilter.html) components to the prefab.
Inherits from `anClipMag`/`anClipObjectEvent`.
## ADSR Event
* This event plays the full ADSR sound envelop using audioClips for `Attack`, `Sustain` and `Release`.

<br/>Start and stop playing the sound by using `Play` and `Stop` on the event/driver
`Play` will play the Attack clip using [AudioSource.PlayOneShot](https://docs.unity3d.com/ScriptReference/AudioSource.PlayOneShot.html) and play the Sustain sound by assigning it to the audio source with loop enabled.
`Stop` will stop the sustain sound at the audio source and play the release sound.
## Speech Event
* This event plays one or more audio clips sequentially.

<br/>Use `Play` to play a sequence. `args` as string array will be used to fetch the audio clips by name, each clip will be scheduled to exactly after the previous clip in the sequence.
Use `PlayScheduled` to play a specified clip at a specified timecode, can be used from the driver. `args[0]` as string is the name of the clip to be played.
## Loop Layers
### LayerMag
* For loop sounds that are meant to be played simultaneously, You can easily instantiate an array of `anSourcerer`s in runtime (you do not use `anDriver`).

<br/>Simply assign an audio clip and the audio source prefab that is used to play it.<br/>
All runtime adjustments are done using your own gameplay scripts.
# Dynamic Music System
## Stem Music Transitions
## Linear Music Transitions
### Synchro Events
# UI
### Button
* Implements sounds for [Button.onClick](https://docs.unity3d.com/530/Documentation/ScriptReference/UI.Button-onClick.html), [OnPointerEnter](https://docs.unity3d.com/2019.1/Documentation/ScriptReference/UI.Selectable.OnPointerEnter.html) and [OnPointerExit](https://docs.unity3d.com/2018.3/Documentation/ScriptReference/UI.Selectable.OnPointerExit.html)
### Slider
* Implements sounds for [Slider.onValueChanged](https://docs.unity3d.com/530/Documentation/ScriptReference/UI.Button-onClick.html), [OnPointerEnter](https://docs.unity3d.com/2019.1/Documentation/ScriptReference/UI.Selectable.OnPointerEnter.html), OnPointerDown, OnPointerUp, [OnPointerExit](https://docs.unity3d.com/2018.3/Documentation/ScriptReference/UI.Selectable.OnPointerExit.html)
