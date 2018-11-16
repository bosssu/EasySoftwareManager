using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class StickClass {

    private byte[] headBuf;
    private byte[] tatalBuf;

    private int headBufLength;      //头部长度
    private int currentRecvLength;  //当前接收的数据长度
    private int alldataLength;      //当前要接受的消息的长度
    private int currentBodyRecvLength;  //

    public delegate void OnOneMsgReceiveFinished(byte[] data);
    private OnOneMsgReceiveFinished onOneMsgReceived;

    public StickClass(int tempHeadBufLength,OnOneMsgReceiveFinished onMsgReceived)
    {
        this.onOneMsgReceived = onMsgReceived;

        headBufLength = tempHeadBufLength;
        headBuf = new byte[headBufLength];
    }

    public void ProcessRecvData(byte[] recv, int realRecvLength)
    {
        if (realRecvLength <= 0)
            return;

        if (currentRecvLength < headBufLength)
        {
            HeadParse(recv, realRecvLength);
        }
        else 
        {
            //当前消息还没有接受完
            if (currentBodyRecvLength + realRecvLength < alldataLength)
            {
                Buffer.BlockCopy(recv, 0, tatalBuf, currentRecvLength, realRecvLength);
                currentRecvLength += realRecvLength;
                currentBodyRecvLength += realRecvLength;
            }

            //当前消息刚好接受完，没有多余的消息
            else if (currentBodyRecvLength + realRecvLength == alldataLength)
            {
                Buffer.BlockCopy(recv, 0, tatalBuf, currentRecvLength, realRecvLength);
                currentRecvLength += realRecvLength;
                currentBodyRecvLength += realRecvLength;

                RecvFinished();
            }

            //上一条消息接受完，但接受的信息中包含了下一条消息的信息
            else
            {
                int partRecv = alldataLength - currentBodyRecvLength;
                Buffer.BlockCopy(recv, 0, tatalBuf, currentRecvLength, partRecv);
                currentRecvLength += partRecv;
                currentBodyRecvLength += partRecv;

                //截取余下的部分
                int tempLength = realRecvLength - alldataLength;

                RecvFinished();

                byte[] temp = new byte[tempLength];
                Buffer.BlockCopy(recv, alldataLength, temp, 0, tempLength);

                ProcessRecvData(temp, tempLength);
            }
        }
    }

    private void HeadParse(byte[] src,int realLength)
    {

        int tempTotalLength = currentRecvLength + realLength;

        if (tempTotalLength <= headBufLength)
        {
            //copy
            Buffer.BlockCopy(src, 0, headBuf, currentRecvLength, realLength);
            currentRecvLength += realLength;
        }
        else
        {
            int headFillLength = headBufLength - currentRecvLength; //填满headbuf需要的字节数

            Buffer.BlockCopy(src, 0, headBuf, currentRecvLength, headFillLength);
            currentRecvLength += headFillLength;

            //head中存储的是body的长度
            alldataLength = BitConverter.ToInt32(headBuf, 0);

            int tatalTempData = alldataLength + headBufLength;
            tatalBuf = new byte[tatalTempData];

            Buffer.BlockCopy(headBuf, 0, tatalBuf, 0, headBufLength);

            //去掉头信息之后的信息
            int remainMsgLength = realLength - headFillLength;
            if (remainMsgLength > 0)
            {
                byte[] bodydata = new byte[remainMsgLength];
                Buffer.BlockCopy(src, headFillLength, bodydata, 0, remainMsgLength);

                ProcessRecvData(bodydata, remainMsgLength);
            }
            else //只接受到了头部信息
            {
                RecvFinished();
            }
        }
    }

    private void RecvFinished()
    {
        if (onOneMsgReceived != null) onOneMsgReceived(tatalBuf);

        currentBodyRecvLength = 0;
        currentRecvLength = 0;
        alldataLength = 0;
        tatalBuf = null;
    }
}

   