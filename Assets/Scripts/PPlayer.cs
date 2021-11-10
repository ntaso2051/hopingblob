using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PPlayer : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Rigidbody rb;
    private bool isJump = false;
    private float sec = 0.0f;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float power;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotSpeed;
    private Vector3 velocity;


    private void Start()
    {
        if (photonView.IsMine)
        {
            photonView.RPC(nameof(SetObjName), RpcTarget.All);
        }
    }

    [PunRPC]
    private void SetObjName()
    {
        this.gameObject.name = $"{photonView.OwnerActorNr}";
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            PlayerInputProcess();
            PlayerUpdate();
        }
    }

    private void PlayerUpdate()
    {
        if (velocity.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(PlayerCamera.Instance.hRotation * velocity * 2), rotSpeed * Time.deltaTime);
        }
        transform.position += PlayerCamera.Instance.hRotation * velocity;

        if (this.transform.position.y < -1.0f || (PUiManager.Instance.EndGameUI.activeSelf && this.gameObject.activeSelf))
        {
            this.gameObject.SetActive(false);
            PlayerCamera.Instance.SetUpGodCamera();
            var pHashtable = new ExitGames.Client.Photon.Hashtable();
            pHashtable["Rank"] = 4 - PNetworkManager.Instance.deadPlayerNum;
            pHashtable["isDead"] = true;
            PhotonNetwork.LocalPlayer.SetCustomProperties(pHashtable);
            PUiManager.Instance.ActivateDeadUI();
        }
    }

    private void PlayerInputProcess()
    {
        if (PNetworkManager.Instance.isGameStarted)
        {
            if (isJump)
            {
                sec += Time.deltaTime;
                if (sec <= 0.1f)
                {
                    Vector3 dir = Vector3.zero;
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Bounce(jumpPower);
                    }
                }
                else
                {
                    sec = 0.0f;
                    isJump = false;
                }
                // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(refCamera.m_hRotation * m_velocity * 2), m_applySpeed);
                // transform.position += refCamera.m_hRotation * m_velocity;
            }
            velocity = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                velocity.z = 1;
            }
            if (Input.GetKey(KeyCode.A))
            {
                velocity.x = -1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                velocity.z = -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                velocity.x = 1;
            }
            velocity = velocity.normalized * moveSpeed * Time.deltaTime;
        }
    }
    private void Bounce(float f)
    {
        Vector3 power = new Vector3(0, f, 0);
        rb.AddForce(power);
        Debug.Log("Bounced");
    }

    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlayJumpSound();
        if (collision.gameObject.tag == "Floor")
        {
            if (photonView.IsMine)
            {
                isJump = true;
                sec = 0.0f;
                this.rb.AddForce(new Vector3(0.0f, 300.0f, 0.0f));
                Debug.Log("Collision Floor");
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            if (photonView.IsMine)
            {
                Vector3 dir = collision.gameObject.transform.position - this.gameObject.transform.position;
                dir.Normalize();
                this.rb.AddForce(-dir * power);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        photonView.RPC(nameof(SetObjName), RpcTarget.All);
    }
}
