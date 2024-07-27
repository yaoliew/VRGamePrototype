using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class GestureProcessor : MonoBehaviour
{
    //A so called "Dictionary" containing all the signs, followed by a SignData containing their current state that takes in int numberOfSteps, bool isTwoHanded, & bool oneTimeActive
    //Essentially maps but with a worse name?  Seriously, what was Microsoft thinking with "Dictionary?"
    public static Dictionary<string, SignData> signs = new Dictionary<string, SignData> {
        {"gun", new SignData(1, false, true, 0)}, 
        {"go", new SignData(2, true, false, 0)},
        {"open", new SignData(2, true, true, 0)},
        {"door", new SignData(3, true, true, 1)}
    };

    public static string curSign;

    public Player player;

    public Transform playerTrans;

    void Update() {
        string tempSign = "";
        //Updates all the timers for the signs 
        for (int i = 0; i < signs.Count; i++) {
            if (signs.ElementAt(i).Value.IsActive()) {
                tempSign = signs.ElementAt(i).Key;
                player.Signed(signs.ElementAt(i).Key);
            }
            signs[signs.ElementAt(i).Key] = signs.ElementAt(i).Value.Update(Time.deltaTime);
        }

        curSign = tempSign;
    }

    //Recieves all gestures from hand and figures out whether it is the next step in the sign given or not
    public void LeftGestured(string str) {
        //Parses the string into more usable data
        ParseSignString(str, out string signName, out List<int> stages);
        //string signName = str.Substring(0, str.Length - 1);
        //int stage = int.Parse(str.Substring(str.Length - 1, 1));

        if (signs.TryGetValue(signName, out SignData signData)) {
            if (signData.IsTwoHanded() && signData.IsNotInDelay()) {
                foreach (int stage in stages) {
                    if (stage == 0 || signData.GetStage() + 1 == stage) {
                        //Tells sign this hand is ready to advance
                        signData.Ready(false);
                        break;
                    } else if (stage == 1) {
                        //Begins the sign anew
                        signData.ResetSign();
                        signData.Ready(false);
                        break;
                    } else if (signData.IsMaxStage() && stage == signData.GetStage()) {
                        //Resets the buffer for this hand of this sign
                        signData.Resign(false);
                        break;
                    }
                }

                //Saves all changes back to the dictionary (which is really just a map)
                signs[signName] = signData;
            }
        } else {
            //In case the sign does not exist in the dictionary (map just sounds so much better)
            Debug.Log("DOESNOTCOMPUTEDOESNOTCOMPUTE");
        }
    }
    public void RightGestured(string str) {
        ParseSignString(str, out string signName, out List<int> stages);

        if (signs.TryGetValue(signName, out SignData signData)) {
            if (signData.IsNotInDelay()) {
                foreach (int stage in stages) {
                    if (stage == 0 || signData.GetStage() + 1 == stage) {
                        signData.Ready(true);
                        break;
                    } else if (stage == 1) {
                        signData.ResetSign();
                        signData.Ready(true);
                        break;
                    } else if (signData.IsMaxStage() && stage == signData.GetStage()) {
                        signData.Resign(true);
                        break;
                    }
                }

                signs[signName] = signData;
            }
        } else {
            Debug.Log("DOESNOTCOMPUTEDOESNOTCOMPUTE");
        }
    }
    
    //Recieves all gestures from hand that are ended and begins the timer for that gestures buffer if appropriate
    public void LeftUngestured(string str) {
        //Parses the string into more usable data
        ParseSignString(str, out string signName, out List<int> stages);

        if (signs.TryGetValue(signName, out SignData signData)) {
            if (signData.IsTwoHanded() && signData.IsNotInDelay()) {
                foreach (int stage in stages) {
                    if (stage == 0 || signData.GetStage() + 1 == stage || (signData.IsMaxStage() && stage == signData.GetStage())) {
                        //Sets the sign to inactive and begins buffer timer until sign resets if not regestured
                        signData.Unsign(false);
                        break;
                    }
                }

                signs[signName] = signData;
            }
        } else {
            Debug.Log("DOESNOTCOMPUTEDOESNOTCOMPUTE");
        }
    }
    public void RightUngestured(string str) {
        ParseSignString(str, out string signName, out List<int> stages);

        if (signs.TryGetValue(signName, out SignData signData)) {
            if (signData.IsNotInDelay()) {
                foreach (int stage in stages) {
                    if (stage == 0 || signData.GetStage() + 1 == stage || (signData.IsMaxStage() && stage == signData.GetStage())) {
                        signData.Unsign(true);
                        break;
                    }
                }

                signs[signName] = signData;
            }
        } else {
            Debug.Log("DOESNOTCOMPUTEDOESNOTCOMPUTE");
        }
    }

    public void Kill(string signName) {
        if (signs.TryGetValue(signName, out SignData signData)) {
            signData.ResetSign();
            signs[signName] = signData;
        } else {
            Debug.Log("DOESNOTCOMPUTEDOESNOTCOMPUTE");
        }
    }

    private void ParseSignString(string str, out string signName, out List<int> stages) {
        int index = str.IndexOf('-');
        signName = str.Substring(0, index);
        stages = new List<int>();
        index++;
        
        while (index < str.Length) {
            stages.Add(int.Parse(str.Substring(index, 1)));
            index++;
        }
    }
}