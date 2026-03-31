using UnityEngine;

public class Drag : MonoBehaviour {
  // credit: https://youtu.be/izag_ZHwOtM?si=BDmxsyPmI2BRc_DI
  // note: the input system -> both
  
  private bool dragging = false;
    private Vector3 offset;

    void Update() {
        if (dragging) {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            // reference to MoodManager.cs, add the node to the "list"
            NodeManager.Instance.UpdateLines(gameObject);
        }
    }

    private void OnMouseDown() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            // if shift is pressed when a node is clicked, then connect them
            NodeManager.Instance.HandleNodeSelection(gameObject);
            return;
        }
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }

    private void OnMouseUp() {
        dragging = false;
    }
}
