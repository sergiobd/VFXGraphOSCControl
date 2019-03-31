# VFXGraphOSCControl
A quick script to control VFX parameters using OSC messages:


1. Import OscJack (https://github.com/keijiro/OscJack/releases)
2. Expose some float parameters using VFX graph's blackboard
3. Add this script under your VisualEffects object
4. Set OSC port, obviously.
5. Add the names of the parameters to the parameters array in the inspector. (Use the EXACT same name in the VFX graph)
6. Send your OSC messages from your preferred application using messages with the structure /<parameter name> <float_value>. 
Eg. "/emission_rate 10000" (emission_rate has to be the exact same name in script and blackboard). 
