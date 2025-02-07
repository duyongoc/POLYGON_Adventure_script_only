using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupOkCancel : PopupBase
{


    [Space(20)]
    [SerializeField] private TMP_Text txtTitle;
    [SerializeField] private TMP_Text txtContent;


    // private
    // private Action callbackButtonOK;
    // private Action callbackButtonCancel;
    // private Action callbackOnShow;
    // private Action callbackOnClose;



    public void Show(string title, string content, Action cbOK = null, Action cbCancel = null)
    {
        // print($"ShowPopupOk title: {title} -  content: {content}");
        txtTitle.text = title.ToUpper();
        txtContent.text = content;
        Show(cbOK, cbCancel);
    }


    // public override void Show(Action cbButtonOK = null, Action cbButtonCancel = null)
    // {
    //     callbackButtonOK = cbButtonOK;
    //     callbackButtonCancel = cbButtonCancel;
    //     Showing();
    // }


    // public override void Hide(Action cbOnClose = null)
    // {
    //     isShowing = false;
    //     Hiding();
    // }


    // private void Showing()
    // {
    //     OnShow();
    //     OnEffectShow();
    // }


    // private void Hiding()
    // {
    //     OnEffectHide(OnClose);
    // }


    // public void OnCallbackButtonOK()
    // {
    //     if (!isShowing)
    //         return;

    //     Hide();
    //     callbackButtonOK?.Invoke();
    // }


    // public void OnCallbackButtonCancel()
    // {
    //     if (!isShowing)
    //         return;

    //     Hide();
    //     isShowing = false;
    //     callbackButtonCancel?.Invoke();
    // }


    // private void OnShow()
    // {
    //     isShowing = true;
    //     popupObject.SetActive(true);
    //     // callbackOnShow?.Invoke();
    // }


    // private void OnClose()
    // {
    //     isShowing = false;
    //     popupObject.SetActive(false);
    //     // callbackOnClose?.Invoke();

    //     // reset action
    //     ResetAction();
    // }


    // private void ResetAction()
    // {
    //     // callbackOnShow = null;
    //     // callbackOnClose = null;
    //     callbackButtonOK = null;
    //     callbackButtonCancel = null;
    // }

}
