using UnityEngine;
using UnityEngine.UI;

public class InfoBar : MonoBehaviour {
    [SerializeField] Slider resource;

    // public bool IsUpdating { get; private set; }
    
    public void SetResource(float resourceNormalized) {
        resource.value = resourceNormalized;
        // resource.transform.localScale = new Vector3(resourceNormalized, 1f);
    }

    // public IEnumerator SetResourceSmooth(float newResourceVal) {
    //     IsUpdating = true;

    //     float curVal = resource.transform.localScale.x;
    //     float changeAmt = curVal - newResourceVal;

    //     while (curVal - newResourceVal > Mathf.Epsilon) {
    //         curVal -= changeAmt * Time.deltaTime;
    //         resource.transform.localScale = new Vector3(curVal, 1f);
    //         yield return null;
    //     }

    //     resource.transform.localScale = new Vector3(newResourceVal, 1f);

    //     IsUpdating = false;
    // }

}
