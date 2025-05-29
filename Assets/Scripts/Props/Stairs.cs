using System;
using Player;
using UnityEngine;

namespace Props
{
    public class Stairs : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var playerRb = other.GetComponent<Rigidbody>();
            var currentConstraints = playerRb.constraints;
            currentConstraints &= ~RigidbodyConstraints.FreezePositionY;
            playerRb.constraints = currentConstraints;
            var playerController = other.GetComponent<PlayerController>();
            playerController.enableDash = false;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var playerRb = other.GetComponent<Rigidbody>();
            var currentConstraints = playerRb.constraints;
            currentConstraints |= RigidbodyConstraints.FreezePositionY;
            playerRb.constraints = currentConstraints;
            var playerController = other.GetComponent<PlayerController>();
            playerController.enableDash = true;
        }
    }
}
