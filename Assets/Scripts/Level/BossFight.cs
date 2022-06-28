using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Level
{
    public class BossFight : MonoBehaviour
    {
        [Tooltip("For testing")] public Stage initialStage;

        public Animator[] computerConsoles;

        [FormerlySerializedAs("onStageDormant")] public UnityEvent onStageDormantEntered;
        public UnityEvent onStageDormantStay;
        public UnityEvent onStageDoorsFailingToCloseEntered;
        public UnityEvent onStageDoorsFailingToCloseStay;
        [FormerlySerializedAs("onStageFirstTentacle")] public UnityEvent onStageFirstTentacleEntered;
        public UnityEvent onStageFirstTentacleStay;

        private Stage stage = Stage.None;
        private readonly Queue<Stage> stageQueue = new Queue<Stage>();
        private readonly bool[] buttonState = new bool[3];
        private int buttonsPressed;

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

            buttonsPressed += buttonState[buttonIndex] ? 1 : -1;

            if (buttonsPressed == 3)
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
                    SetStage(Stage.DoorsFailingToClose, Stage.FirstTentacle);
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
        }
    }
}