using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Audio;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Level
{
    public class BossFight : MonoBehaviour
    {
        [Tooltip("For testing")] public Stage[] initialStage;

        public AudioClipWithVolume buttonPressed;

        public Animator[] computerConsoles;

        [FormerlySerializedAs("onStageDormant")]
        public UnityEvent onStageDormantEntered;

        public UnityEvent onStageDoorsFailingToCloseEntered;

        [FormerlySerializedAs("onStageFirstTentacle")]
        public UnityEvent onStageFirstTentacleEntered;

        public UnityEvent onWaitingForSecondAttempt;

        public UnityEvent onSecondTentacleEnter;

        public UnityEvent onWaitingForThirdAttempt;
        
        [FormerlySerializedAs("onBossRevealEnter")]
        public UnityEvent onGrabWall;

        public UnityEvent onTearDownWall;

        public UnityEvent onBossEngaged;

        public UnityEvent onOneDisabledConsole;
        public UnityEvent onTwoDisabledConsoles;
        public UnityEvent onThreeDisabledConsoles;

        public UnityEvent onClosingDoors;
        public UnityEvent onKillBoss;
        public UnityEvent onFightOver;

        public UnityEvent onGameCompleted;
        
        [SerializeField] private Stage stage = Stage.None;
        private readonly Queue<Stage> stageQueue = new Queue<Stage>();
        private readonly bool[] buttonState = new bool[3];
        private readonly bool[] disabledState = new bool[3];
        private AudioSource audioSource;

        private void OnEnable()
        {
            audioSource = GetComponent<AudioSource>();
            ResetButtons();
            SetStage(initialStage);
        }

        private void FixedUpdate()
        {
            for (var buttonIndex = 0; buttonIndex < computerConsoles.Length; buttonIndex++)
            {
                computerConsoles[buttonIndex].SetBool("DoorControlActive", buttonState[buttonIndex]);
            }

            CheckStage();
        }

        private void CheckStage()
        {
            if (stage == Stage.None && stageQueue.Count > 0)
            {
                stage = stageQueue.Dequeue();
                OnStageEntered(stage);
            }
        }

        private void SetStage(params Stage[] stages)
        {
            foreach (var s in stages)
            {
                stageQueue.Enqueue(s);
            }
        }

        public void AdvanceToNextStage(float delay)
        {
            if (delay > 0) StartCoroutine(DelayedSetStageCoroutine(delay));
            else stage = Stage.None;
        }

        public void TearDownWall()
        {
            SetStage(Stage.TearDownTheWall, Stage.BossEngaged);
            AdvanceToNextStage(0);
        }

        public void DisableComputer(int index)
        {
            if (disabledState[index]) return;
            
            disabledState[index] = true;
            var numDisabled = disabledState.Count(s => s);
            switch (numDisabled)
            {
                case 1: 
                    onOneDisabledConsole.Invoke();
                    break;
                case 2:
                    onTwoDisabledConsoles.Invoke();
                    break;
                case 3:
                    onThreeDisabledConsoles.Invoke();
                    break;
            }
        }

        public void CloseIrisDoors(float delay)
        {
            SetStage(Stage.ClosingDoors);
            AdvanceToNextStage(delay);
        }

        public void KillBoss(float delay)
        {
            SetStage(Stage.WaitingForBossToDie);
            AdvanceToNextStage(delay);
        }

        public void FightOver(float delay)
        {
            SetStage(Stage.FightOver);
            AdvanceToNextStage(delay);
        }

        public void GameCompleted(float delay)
        {
            SetStage(Stage.GameCompleted);
            AdvanceToNextStage(delay);
        }

        public void Unmask(SpriteRenderer sr)
        {
            sr.maskInteraction = SpriteMaskInteraction.None;
        }

        private IEnumerator DelayedSetStageCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            stage = Stage.None;
        }

        private void OnStageEntered(Stage s)
        {
            switch (s)
            {
                case Stage.Dormant:
                    onStageDormantEntered.Invoke();
                    break;
                case Stage.DoorsFailingToClose:
                    onStageDoorsFailingToCloseEntered.Invoke();
                    break;
                case Stage.FirstTentacle:
                    onStageFirstTentacleEntered.Invoke();
                    break;
                case Stage.SecondTentacle:
                    onSecondTentacleEnter.Invoke();
                    break;
                case Stage.WaitingForSecondAttempt:
                    onWaitingForSecondAttempt.Invoke();
                    break;
                case Stage.WaitingForThirdAttempt:
                    onWaitingForThirdAttempt.Invoke();
                    break;
                case Stage.GrabWall:
                    onGrabWall.Invoke();
                    break;
                case Stage.TearDownTheWall:
                    onTearDownWall.Invoke();
                    break;
                case Stage.BossEngaged:
                    onBossEngaged.Invoke();
                    break;
                case Stage.ClosingDoors:
                    onClosingDoors.Invoke();
                    break;
                case Stage.WaitingForBossToDie:
                    onKillBoss.Invoke();
                    break;
                case Stage.FightOver:
                    onFightOver.Invoke();
                    break;
                case Stage.GameCompleted:
                    onGameCompleted.Invoke();
                    break;
            }
        }


        public void PressButton(int buttonIndex)
        {
            buttonState[buttonIndex] = !buttonState[buttonIndex];
            audioSource.PlayOneShot(buttonPressed);

            if (buttonState.All(x => x))
            {
                ActivateIrisDoor();
            }
        }

        public void ResetButtons()
        {
            for (var buttonIndex = 0; buttonIndex < 3; buttonIndex++)
            {
                buttonState[buttonIndex] = false;
            }
        }

        private void ActivateIrisDoor()
        {
            switch (stage)
            {
                case Stage.Dormant:
                    SetStage(Stage.DoorsFailingToClose, Stage.FirstTentacle, Stage.WaitingForSecondAttempt);
                    AdvanceToNextStage(0);
                    break;
                case Stage.WaitingForSecondAttempt:
                    SetStage(Stage.DoorsFailingToClose, Stage.SecondTentacle, Stage.WaitingForAllEnemiesToDie,
                        Stage.WaitingForThirdAttempt);
                    AdvanceToNextStage(0);
                    break;
                case Stage.WaitingForThirdAttempt:
                    SetStage(Stage.DoorsFailingToClose, Stage.GrabWall);
                    AdvanceToNextStage(0);
                    break;
            }
        }

        [Serializable]
        public enum Stage
        {
            None,
            Dormant,
            DoorsFailingToClose,
            FirstTentacle,
            WaitingForSecondAttempt,
            SecondTentacle,
            WaitingForAllEnemiesToDie,
            WaitingForThirdAttempt,
            GrabWall,
            TearDownTheWall,
            BossEngaged,
            ClosingDoors,
            WaitingForBossToDie,
            FightOver,
            GameCompleted
        }
    }
}