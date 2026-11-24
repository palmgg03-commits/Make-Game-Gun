using System.Collections;
using TMPro;
using UnityEngine;

public class Mission : MonoBehaviour
{
    //maybe have something to save inventory data so it can link with this script
    [System.Serializable]
    public class CollectionTask
    {
        public string thingsTag;
        public int targetAmount;
        public int currentAmount;
    }

    public CollectionTask[] missionList;
    public TMP_Text mustText;


    public void EnterGoal()
    {
        //winUI
    }

    public IEnumerator TimerEnter(string mustFinishText)
    {
        mustText.text = mustFinishText;
        mustText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        mustText.gameObject.SetActive(false);
        mustText.text = "";
    }


}
