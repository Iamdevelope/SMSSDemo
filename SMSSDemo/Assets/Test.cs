using cn.SMSSDK.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour, SMSSDKHandler
{

    public SMSSDK ssdk;
    private InputField code;
    private InputField phoneNum;
    private Button enter;
    private Button send;
    private string codeNum;
    private string phone;
    private Text timer;
    private bool isSend;
    private int time;
    private float t;
    public Text text;
    private void Start()
    {
        ssdk.init("292449f735890", "f1bee8045aac2e6cbb7c535a5277aa1c", false);
        ssdk.setHandler(this);
        timer = transform.Find("Timer").GetComponent<Text>();
        code = transform.Find("code").GetComponent<InputField>();
        phoneNum = transform.Find("num").GetComponent<InputField>();
        enter = transform.Find("enter").GetComponent<Button>();
        send = transform.Find("send").GetComponent<Button>();

        timer.gameObject.SetActive(false);
        enter.onClick.AddListener(EnterCodeHandler);
        send.onClick.AddListener(SendCodeHandler);
    }
    private void Update()
    {
        if (isSend)
        {
            //倒计时
            timer.text = time.ToString();
            t += Time.deltaTime;
            if (t >= 1)
            {
                time--;
                t = 0;
            }
            if (time < 0)
            {
                isSend = false;
                send.gameObject.SetActive(true);

                timer.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 发送验证码
    /// </summary>
    private void SendCodeHandler()
    {
        phone = phoneNum.text;
        isSend = true;
        time = 60;
        send.gameObject.SetActive(false);
        timer.gameObject.SetActive(true);
        ssdk.getCode(CodeType.TextCode, phone, "86", null);
    }
    /// <summary>
    /// 点击确定，对比验证码
    /// </summary>
    private void EnterCodeHandler()
    {
        ssdk.commitCode(phone, "86", code.text);
    }

    public void onComplete(int action, object resp)
    {
        ActionType act = (ActionType)action;
        if (resp != null)
        {
            //result = resp.ToString();
            text.text += "\n" + resp.ToString();
            Debug.Log(resp.ToString());
        }
        if (act == ActionType.GetCode)
        {
            text.text += "\n 验证成功!!!";
            string responseString = (string)resp;
            Debug.Log("isSmart :" + responseString);
        }
    }

    public void onError(int action, object resp)
    {
        Debug.Log("Error :" + resp);
        text.text += "\n 验证失败!!!";
        text.text += "\n Error : " + resp;
        print("OnError ******resp" + resp);
    }
}
