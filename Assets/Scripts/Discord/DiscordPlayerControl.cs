using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscordPlayerControl : MonoBehaviour {

    public Transform discordPlayerPrefab;

    private Vector3 spawnOffset = new Vector3(0,2,0);
    
    public Dictionary<string, DiscordPlayerData> discordPlayers = new Dictionary<string, DiscordPlayerData>();
    public List<string> playerNames;

    [SerializeField]
    public class DiscordPlayerData
    {
        public string name;
        public Transform avatar;
        public Text namebadge;
        public Color color;
        public int mana;
    }

    public Text namePrefab;
    public RectTransform uiContainer;
    public int nameCount;
    public int namePerPage = 25;
    public float nameWidth = 150;

    public float manaInfusionTime=10;
    private float timer;

    private const int CHAOS_CONTROL = 0;
    private const int METEOR = 1;
    private const int FLING = 2;
    private const int RAPID_FIRE=3;
    public int[] spellCost = { 250, 150, 100 };
    public static float chaos_control_active = 0;
    public float chaosControlDuration = 2.5f;

    public static float rapid_fire_active = 0;
    public float rapidFireDuration = 2.5f;

    public Transform MeteorPrefab;


    //for testing
    public string[] nameList = { "Alpha", "Beta", "Gamma", "Delta", "Epsilon" };



	// Use this for initialization
	void Start () {
	/*	if (discordPlayers.Count > 0)
        {
            foreach (string name in discordPlayers)
            {
                SpawnPlayer(name);
            }
        }*/
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("TEST_ADD_PLAYER"))
        {
            AttemptSpell("Accipiter Chalybs", CHAOS_CONTROL, 0, 0);
          //  AddPlayer(nameList[Random.Range(0, 5)]);
          //  if (Random.RandomRange(0, 10) == 0) SetColor(nameList[Random.Range(0, 5)], new Color(0, 0.5f, 1));
        }

        timer -= Time.deltaTime;
        if (timer<0)
        {
            print("Timer magic");
            foreach (string name in playerNames)
            {
                DiscordPlayerData data = getPlayer(name); if (data == null) return;
                data.mana += 100;
                UpdateName(name);
            }
            timer = manaInfusionTime;
        }

        if (chaos_control_active > 0)
        {
            chaos_control_active -= Time.deltaTime;
        }

        if (rapid_fire_active > 0)
        {
            rapid_fire_active -= Time.deltaTime;
        }
	}


    public void handleMessage(DiscordManager.DiscordMessage m)
    {
        print("Message from " + m.author + ": " + m.content);
        string name = m.author;
        if (name.Length > 20)
        {
            name = name.Substring(0, 20);
        }

        if (getPlayer(name) == null)
        {
            AddPlayer(name);
        }

        if (getPlayer(name) != null)
        {
            string content = m.content;
            string[] words = content.ToLower().Split(' ');
            if (words[0]=="colour" || words[0]=="color")
            {
                if (words.Length != 4) return;
                int r=0, g=0, b=0;
                if (!System.Int32.TryParse(words[1], out r)) return;
                if (!System.Int32.TryParse(words[2], out g)) return;
                if (!System.Int32.TryParse(words[3], out b)) return;
                if (r > 255 || g > 255 || b > 255 || r < 0 || g < 0 || b < 0) return;
                SetColor(name, r, g, b);
            }
            if ((words[0]=="chaos" && words[1]=="control") || words[0]=="chaoscontrol")
            {
                AttemptSpell(name, CHAOS_CONTROL, 0, 0);
            }
            if (words[0]=="meteor")
            {
                int x = 0, y = 0;
                if (words[1].Length == 0) return;
                switch (words[1][0])
                {
                    case 'a':
                        x = 0;
                        break;
                    case 'b':
                        x = 1;
                        break;
                    case 'c':
                        x = 2;
                        break;
                    case 'd':
                        x = 3;
                        break;
                    case 'e':
                        x = 4;
                        break;
                    case 'f':
                        x = 5;
                        break;
                    case 'g':
                        x = 6;
                        break;
                    case 'h':
                        x = 7;
                        break;
                    case 'i':
                        x = 8;
                        break;
                    case 'j':
                        x = 9;
                        break;
                    default:
                        if (!System.Int32.TryParse(words[1], out x)) return;
                        break;

                }
                if (words[1].Length > 1)
                {
                    if (!System.Int32.TryParse(words[1].Substring(1), out y)) return;
                } else
                {
                    if (words.Length < 3 || !System.Int32.TryParse(words[2], out y)) return;
                }
                if (x > 10 || y > 10 || x < 0 || y < 0) return;
                AttemptSpell(name, METEOR, x, y);
            }
            if (words[0] == "fling" || words[0] == "jump")
            {
                AttemptSpell(name, FLING, 0, 0);
            }
            if (words[0] == "rapidfire" || words[0] == "rapid")
            {
                AttemptSpell(name, RAPID_FIRE, 0, 0);
            }
        }
    }

    public void AddPlayer(string name)
    {
        string attemptName = name;
        int tryCount = 0;
        while (getPlayer(attemptName) != null)
        {
            attemptName = name + "(" + tryCount++ + ")";
        }
        name = attemptName;


        DiscordPlayerData playerData = new DiscordPlayerData();
        playerData.name = name;
        playerData.color = new Color(1, 1, 0);
        playerData.mana = 0;
        playerData.avatar = Instantiate(discordPlayerPrefab, this.transform.position + spawnOffset, Quaternion.identity);


        Text namebadge = Instantiate(namePrefab);
        namebadge.rectTransform.SetParent(uiContainer);
        namebadge.rectTransform.anchoredPosition = new Vector2(-325+nameWidth*(nameCount/namePerPage), 261 - 25 * (nameCount++%namePerPage));
        playerData.namebadge = namebadge;

        playerNames.Add(name);
        discordPlayers[name] = playerData;

        UpdateName(name);
    }

    public void SetColor(string name, int r, int g, int b)
    {
        Color c = new Color(r / 255.0f, g / 255.0f, b / 255.0f);
        SetColor(name, c);
    }

    public void SetColor(string name, Color c) {
        DiscordPlayerData data = getPlayer(name); if (data == null) return;
        data.color = c;
        data.avatar.GetComponent<Recolour>().GenerateMaterial(c);
        UpdateName(name);
    }

    public void UpdateName(string name)
    {
        DiscordPlayerData data = getPlayer(name); if (data == null) return;
        data.namebadge.text = name + " | " + data.mana;
        data.namebadge.color = data.color;
    }

    public void AttemptSpell(string name, int spell, int arg1, int arg2)
    {
        DiscordPlayerData data = getPlayer(name); if (data == null) return;
        print(spell);
        switch (spell)
        {
            case CHAOS_CONTROL:
                if (data.mana < spellCost[CHAOS_CONTROL]) return;

                data.mana -= spellCost[CHAOS_CONTROL];

                chaos_control_active += chaosControlDuration;

                UpdateName(name);
                break;
            case METEOR:
                if (data.mana < spellCost[METEOR]) return;
                data.mana -= spellCost[METEOR];

                Instantiate(MeteorPrefab, new Vector3(arg1 * 10 - 50, 50, arg2 * 10 - 50), Quaternion.Euler(90,0,0));
                UpdateName(name);
                break;

            case FLING:
                if (data.mana < spellCost[FLING]) return;
                data.mana -= spellCost[FLING];

                this.GetComponent<Rigidbody>().AddForce(new Vector3(0, 25, 0), ForceMode.Impulse);
                UpdateName(name);
                break;
            case RAPID_FIRE:

                if (data.mana < spellCost[RAPID_FIRE]) return;
                data.mana -= spellCost[RAPID_FIRE];

                rapid_fire_active += rapidFireDuration;
                UpdateName(name);
                break;
        }
    }

    public DiscordPlayerData getPlayer(string name)
    {
        try
        {
            return discordPlayers[name];
        }
        catch (System.Exception e)
        {
            return null;
        }
    }
}
