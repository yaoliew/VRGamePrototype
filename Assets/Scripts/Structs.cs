using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SignData {
    private int stages;
    private bool isTwoHanded;
    private int curStage;
    private bool isLeftReady;
    private bool isRightReady;
    private bool isActive;
    private bool isActiveOnce;
    private int staticHand;

    private float timer;
    private float rightTimer;
    private float leftTimer;
    private float signDelay;
    //Debug Only
    //private int debugDelay;

    public SignData(int stages, bool isTwoHanded, bool isActiveOnce, int staticHand) {
        this.stages = stages;
        this.isTwoHanded = isTwoHanded;
        this.curStage = 0;
        this.isLeftReady = false;
        this.isRightReady = false;
        this.isActive = false;
        this.isActiveOnce = isActiveOnce;
        this.staticHand = staticHand;
        this.timer = 0f;
        this.rightTimer = 0f;
        this.leftTimer = 0f;
        this.signDelay = 0f;
        //this.debugDelay = 0;
    }

    public SignData Update(float deltaTime) {
        if (isActive) {
            if (isActiveOnce) {
                ResetSign();
            }
            
        } else if (timer > 0) {
            timer -= deltaTime;

            if (timer <= 0) {
                ResetSign();
            }
        } else if (signDelay > 1) {
            signDelay -= deltaTime;
        }

        if (rightTimer > 0) {
            rightTimer -= deltaTime;

            if (rightTimer <= 0) {
                if (curStage == stages) {
                    ResetSign();
                    signDelay = 1f;
                } else {
                    isRightReady = false;
                }
            }
        }
        if (leftTimer > 0) {
            leftTimer -= deltaTime;

            if (leftTimer <= 0) {
                if (curStage == stages) {
                    ResetSign();
                } else {
                    isLeftReady = false;
                }
            }
        }

        /*debugDelay++;
        if (debugDelay >= 10) {
            Debug.Log("Timer: " + timer + "    RightTimer: " + rightTimer + "    LeftTimer: " + leftTimer);
            debugDelay = 0;
        }*/

        return this;
    }

    public void IncStage() {
        if (curStage < stages && ++curStage == stages) {
            isActive = true;
        } else {
            timer = 4f;

            if (staticHand == 0) {
                isRightReady = false;
                isLeftReady = false;
            } else if (staticHand == 1) {
                isRightReady = false;
            } else {
                isLeftReady = false;
            }
        }
    }

    public void ResetSign() {
        curStage = 0;
        isRightReady = false;
        isLeftReady = false;
        isActive = false;
        timer = 0f;
        rightTimer = 0f;
        leftTimer = 0f;
    }

    public void Ready(bool isRight) {
        if (isRight) {
            isRightReady = true;

            if (rightTimer > 0) {
                rightTimer = 0f;
            }
        } else {
            isLeftReady = true;

            if (leftTimer > 0) {
                leftTimer = 0f;
            }
        }

        if (!isTwoHanded) {
            IncStage();
        } else if (isRightReady && isLeftReady) {
            IncStage();
        }
    }

    public void Unsign(bool isRight) {
        isActive = false;
        
        if (isRight) {
            rightTimer = 1f;
        } else {
            leftTimer = 1f;
        }
    }

    public void Resign(bool isRight) {
        if (isRight) {
            rightTimer = 0f;
        } else {
            leftTimer = 0f;
        }

        if (leftTimer == 0 && rightTimer == 0) {
            isActive = true;
        }
    }

    public int GetStage() {
        return curStage;
    }

    public bool IsMaxStage() {
        return curStage == stages;
    }

    public bool IsTwoHanded() {
        return isTwoHanded;
    }

    public bool IsNotInDelay() {
        return signDelay <= 0;
    }

    public bool IsActive() {
        return isActive;
    }
}