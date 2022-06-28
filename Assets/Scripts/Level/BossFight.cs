using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Level
{
    public class BossFight : MonoBehaviour
    {
        [Tooltip("For testing")] public Stage initialStage;

        public Animator[] computerConsoles;

        [FormerlySerializedAs("onStageDormant")]
        public UnityEvent onStageDormantEntered;

        public UnityEvent onStageDormantStay;
        public UnityEvent onStageDoorsFailingToCloseEntered;
        public UnityEvent onStageDoorsFailingToCloseStay;

        [FormerlySerializedAs("onStageFirstTentacle")]
        public UnityEvent onStageFirstTentacleEntered;

        public UnityEvent onStageFirstTentacleStay;
        public UnityEvent onSecondTentacleEnter;

        [FormerlySerializedAs("onBossRevealEnter")]
        public UnityEvent onGrabWall;

        public UnityEvent onTearDownWall;

        [SerializeField] private Stage stage = Stage.None;
        private readonly Queue<Stage> stageQueue = new Queue<Stage>();
        private readonly bool[] buttonState = new bool[3];

        private void OnEnable()
        {
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
                Debug.Log("New stage: " + stage);
                OnStageEntered(stage);
            }
            else
            {
                OnStageStay(stage);
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
            SetStage(Stage.TearDownTheWall, Stage.BossReveal, Stage.FightOver);
            AdvanceToNextStage(0);
        }

        private IEnumerator DelayedSetStageCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            Debug.Log("Advance to next stage...");
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
                case Stage.GrabWall:
                    onGrabWall.Invoke();
                    break;
                case Stage.TearDownTheWall:
                    onTearDownWall.Invoke();
                    break;
            }
        }

        private void OnStageStay(Stage s)
        {
            switch (s)
            {
                case Stage.Dormant:
                    onStageDormantStay.Invoke();
                    break;
                case Stage.DoorsFailingToClose:
                    onStageDoorsFailingToCloseStay.Invoke();
                    break;
                case Stage.FirstTentacle:
                    onStageFirstTentacleStay.Invoke();
                    break;
            }
        }

        public void PressButton(int buttonIndex)
        {
            buttonState[buttonIndex] = !buttonState[buttonIndex];

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
            BossReveal,
            WaitingForBossToDie,
            FightOver
        }
    }
}