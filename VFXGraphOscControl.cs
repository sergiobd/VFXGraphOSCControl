/*

A quick script for controlling VFX parameters using OSC messages

1. Import OscJack (https://github.com/keijiro/OscJack/releases)
2. Expose some float parameters using VFX graph's blackboard
3. Add this script under your VisualEffects object
4. Set OSC port, obviously.
5. Add the names of the parameters to the parameters array in the inspector. (Use the EXACT same name in the VFX graph)
6. Send your OSC messages from your preferred application using the messages with the structure /<parameter name> <float_value>. 
Eg. "/emission_rate 10000" (emission_rate has to be the exact same name in script and blackboard). 


*/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.VFX;
using OscJack;

[RequireComponent(typeof(VisualEffect))]
public class VFXGraphOscControl : MonoBehaviour
{

    public int port = 9001;

    OscServer server;

    VisualEffect visualEffect;


    //[Tooltip("Add a name for each (float) exposed parameter on your VFX graph")]
    public string [] parameters;

    private Dictionary<string, bool> updateFlags;
    private Dictionary<string, float> valueHolder;
    

    void Start()
    {
        server = new OscServer( port);

        updateFlags = new Dictionary<string, bool>();

        valueHolder = new Dictionary<string, float>();
        
        visualEffect = GetComponent<VisualEffect>();
        
        foreach(string parameter in parameters)
        {
            string callbackName = "/" + parameter;

            updateFlags.Add(parameter, false);

            valueHolder.Add(parameter, 0f);
        
            server.MessageDispatcher.AddCallback(callbackName,
            (string address, OscDataHandle data) =>
            {
                string parameterName = address.Substring(1);

                updateFlags[parameterName] = true;
               
                valueHolder[parameterName] = data.GetElementAsFloat(0);
               
            }
            );

            Debug.Log("Added callback for " + callbackName);
        }
        

    }

    void Update()
    {

        foreach (string parameter in parameters) {

            if (updateFlags[parameter] == true) {

                visualEffect.SetFloat(parameter, valueHolder[parameter]);

                updateFlags[parameter] = false;

                //Debug.Log("setting " + parameter);
                

            }

        }

    }
    
    
    void OnDisable() {

        server.Dispose();

    }
}
