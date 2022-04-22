using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class CameraPlacement : MonoBehaviour
{
    [SerializeField] private Transform player, forwardObject;

    private Vector3 normal;
    private Vector3 point;
    private Vector3 direction;

    [SerializeField] private float cameraMoveBuffer = 0;
    private float cameraHeight = 12;

    private Ray playerRay;
    [SerializeField] private LayerMask sphereLayer;



    void FixedUpdate()
    {
        if (!Player.dead)
        {
            DistanceOffGround();
            Look();
        }

    }

    private void Look()
    {
        //aim camera at player

        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation((player.position - transform.position).normalized, forwardObject.transform.forward),
            Time.fixedDeltaTime * 12);
    }

    private void DistanceOffGround()
    {
        //position camera above player

        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");
        direction = new Vector3(moveHorizontal, moveVertical, 0).normalized;

        if (direction.magnitude < 0.1f)
            playerRay = new Ray(player.position, (Player.sphere.transform.position - player.transform.position)); //cast from player to center of object below
        else
            playerRay = new Ray(player.position + (transform.TransformDirection(direction) * cameraMoveBuffer),
                (Player.sphere.transform.position - player.transform.position)); //if player is moving, put camera slightly in front of them

        RaycastHit playerInfo;

        if (Physics.Raycast(playerRay, out playerInfo, 50, sphereLayer))
        {
            //take point on sphere and place camera at cameraHeight above point

            point = playerInfo.point;
            normal = playerInfo.normal;

            Vector3 newPosition = (point + normal * cameraHeight);

            transform.position = Vector3.Slerp(transform.position, newPosition, Time.fixedDeltaTime * 2);
                
        }
    }


}
