using UnityEngine;
using UnityEngine.UI;

public class BusTicketsPanel : MonoBehaviour, INeedDriver
{
    public Driver driver { get; set; }

    [SerializeField]
    Button quitFromJailTicket;
    [SerializeField]
    Button[] thirdDieRollTickets;
    [SerializeField]
    GameObject ticketsPool;
    // những pool này có thể được cấu hình bằng SO và khởi tạo runtime để dễ expand
    [SerializeField]
    GameObject ticketList;

    void Start()
    {
        driver.OnKeepTheTicket(AddToTicketList);

        quitFromJailTicket.onClick.AddListener(
            () => UseCard(quitFromJailTicket.gameObject,
                (byte)BusTicketsConfig.KeepToUseTicket.QuitFromJail)
        );
        for (int i = 0; i < thirdDieRollTickets.Length; i++)
        {
            int index = i;
            //func và action lưu biến không-phải-tham-số-theo-chữ-ký dưới dạng tham chiếu
            //biến i được khởi tạo một lần từ trước nên nếu truyền i, chỉ lấy được giá trị cuối được lưu của i
            //phải khởi tạo mới sau mỗi lần lặp để giữ các tham chiếu đến các biến index khác nhau
            thirdDieRollTickets[i].onClick.AddListener(
                () => UseCard(thirdDieRollTickets[index].gameObject,
                    (byte)BusTicketsConfig.KeepToUseTicket.ThirdDieRoll)
            );
        }
        gameObject.SetActive(false);
    }

    void UseCard(GameObject ticketObj, int ticketValue)
    {
        driver.UseTicketInKeep(0, ticketValue);
        ticketObj.transform.SetParent(ticketsPool.transform);
    }

    void AddToTicketList(BusTicketsConfig.KeepToUseTicket ticketName)
    {
        switch (ticketName)
        {
            case BusTicketsConfig.KeepToUseTicket.QuitFromJail:
                quitFromJailTicket.transform.SetParent(ticketList.transform);
                return;
            case BusTicketsConfig.KeepToUseTicket.ThirdDieRoll:
                TakeAThirdDieRollTicket();
                return;
        }

    }

    void TakeAThirdDieRollTicket()
    {
        for (int i = 0; i < thirdDieRollTickets.Length; i++)
        {
            if (!thirdDieRollTickets[i].gameObject.activeInHierarchy)
            {
                thirdDieRollTickets[i].transform.SetParent(ticketList.transform);
                return;
            }
        }
    }

    void OnDestroy()
    {
        quitFromJailTicket.onClick.RemoveAllListeners();
        for (int i = 0; i < thirdDieRollTickets.Length; i++)
        {
            thirdDieRollTickets[i].onClick.RemoveAllListeners();
        }
    }

}