using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SQLiteDatabase;
using TMPro;

/// <summary>
/// This class is used to set properties for each headstone.
/// </summary>
/// <remarks>
/// The name of the headstone gameObject contains the burialID,
/// which allows the database and proper materials to be used.
/// </remarks>
public class HSdata : MonoBehaviour
{
    /// <summary>
    ///  Flag for HeadstoneLoaderScript to see if it's currently being collided with by the scanner
    /// </summary>
    public bool inRange = false;
    /// <summary>
    /// Holds the reference to the databaseControl script.
    /// </summary>
    DatabaseControl databaseControl;

    /// <summary>
    /// Holds the current headstone's branch of service
    /// </summary>
    public string hsBranch;

    /// <summary>
    /// Holds a reference to the stationary biography toggle status
    /// </summary>
    public bool bioStationary;

    // define the variables for the headstone materials
    private Renderer _renderHS;
    private Renderer _renderButton;
    private Renderer _renderBio;
    /// <summary>
    /// Holds the texture for the new headstone.
    /// </summary>
    public Texture2D texHSNew;
    /// <summary>
    /// Holds the texture for the branch of service button.
    /// </summary>
    public Texture2D texBranch;
    /// <summary>
    ///  Holds the texture for the biography board for ones with a material option.
    /// </summary>
    public Texture2D texBioMaterial;

    /// <summary>
    /// Flag to track whether or not the bio has been built already
    /// </summary>
    public bool loadedBio = false;

    private string _strBioMatFileName;
    private string _strFaceFileName;
    private string _strBranchFileName;
    List<string> buildItems = new List<string>();
    private Transform rotPiece;
    private Transform child;
    private Transform gchild;
    private Transform ggchild;
    public bool initialized = false;
    /// <summary>
    /// Retrieves the reference to the database control script.
    /// </summary>
    
    // Calls awake remotely from DatabaseControl, this script should always be "disabled." 
    // This is janky but you do what you must.
    public void callAwake()
    {
        Awake();
    }

    private void Awake()
    {
        //Return if we've done this already
        if (initialized) return;

        _renderHS = gameObject.transform.Find("Headstone0").GetComponent<Renderer>();
        // get a reference to the already opened database through the control script
        databaseControl = GameObject.Find("DatabaseControl").GetComponent<DatabaseControl>();
        //default the bio board to stationary
        bioStationary = true;
    }

    /// <summary>
    /// Retrieves all of the data for the headstone and biography board.
    /// </summary>
    /// <remarks>
    /// This processing also creates the biography board text for those headstones 
    /// which lack a material level biography.
    /// </remarks>
    public void LoadBio()
    {
        // get the identification for the headstone
        var headStone = gameObject.name.Trim();

        TMP_Text biography;
        string lifeYears = "", branches = "", conflicts = "", awards = "", sex = "", mainBranch = "";
        int bYear, dYear, numItems, cnt, addPeriod;
        bool bioDone;

        // get the biography component
        rotPiece = transform.Find("BillboardRotation");
        child = rotPiece.transform.Find("bioBoardBack");
        gchild = child.Find("Canvas");
        ggchild = gchild.Find("biography");
        // get the text for the biography
        biography = ggchild.GetComponent<TMP_Text>();

        // build the SQL query based on the name of the headstone gameobject
        // as that is where the burial ID is stored. We want all names on the headstone.
        // bIDstr would have the section and site
        string bIDstr = headStone.Substring(0,headStone.Length - 2);

        string[] hsPieces = bIDstr.Split('-');
        if (hsPieces.Length > 2)
        {
            // need to put the A/B lettering back on
            hsPieces[1] += "-" + hsPieces[2];
        }
        var sqlStr = "SELECT * FROM cemVRburials WHERE section == '" + hsPieces[0] + "' and site == '" + hsPieces[1] + "' ORDER BY markerFace";

        // query the database for this headstone
        DBReader reader = databaseControl.db.Select(sqlStr);
        biography.text = "";
        bioDone = false;
        while (reader != null && reader.Read())
        {
            // first check to see if a biography material exists for this headstone
            // it is stored in the headstonePhoto field
            if (reader.GetStringValue("headstonePhoto") != "")
            {
                StartCoroutine(GetBioMaterialTexture());
                bioDone = true;
            }
            else if (!bioDone)
            {
                // only build biography text if there wasn't a material bio
                // build the information into paragraphs for the biography panel
                // start with the full name centered across the top
                biography.text += "<size=200%><align=center><b><color=blue>" + reader.GetStringValue("firstName") + " " + (reader.GetStringValue("middleName").Length > 1 ? reader.GetStringValue("middleName") : titleCase(reader.GetStringValue("middleName"))) + " " + reader.GetStringValue("lastName") + " " + reader.GetStringValue("suffix") + "</b></color></align></size>";

                bYear = reader.GetIntValue("birthYear");
                dYear = reader.GetIntValue("deathYear");
                if (bYear > 0 && dYear > 0)
                {
                    // make the full date span
                    lifeYears = "(" + bYear.ToString() + "-" + dYear.ToString() + ")";
                }
                else if (bYear > 0 && dYear == 0)
                {
                    lifeYears = "(b. " + bYear.ToString() + ")";
                }
                else if (bYear == 0 && dYear > 0)
                {
                    lifeYears = "(d. " + dYear.ToString() + ")";
                }

                if (lifeYears != "")
                {
                    biography.text += "<br><align=center>" + lifeYears + "</align><br>";
                }

                if (reader.GetStringValue("sex") == "M")
                {
                    sex = "He";
                }
                else
                {
                    sex = "She";
                }

                if (bYear > 0)
                {
                    biography.text += "<br>" + titleCase(reader.GetStringValue("firstName")) + " " + titleCase(reader.GetStringValue("middleName")) + " " + titleCase(reader.GetStringValue("lastName"));

                    if (reader.GetStringValue("suffix") != "")
                    {
                        biography.text += ", " + titleCase(reader.GetStringValue("suffix")) + ".";
                    }
                    biography.text += " was born in " + bYear.ToString() + ". ";
                }

                if (reader.GetIntValue("veteran") == 1)
                {
                    if (bYear > 0)
                    {
                        biography.text += sex;
                    }
                    else
                    {
                        // start the sentence with their name
                        biography.text += "<br>" + titleCase(reader.GetStringValue("firstName")) + " " + titleCase(reader.GetStringValue("middleName")) + " " + titleCase(reader.GetStringValue("lastName"));
                        if (reader.GetStringValue("suffix") != "")
                        {
                            biography.text += ", " + titleCase(reader.GetStringValue("suffix")) + ".";
                        }
                    }

                    biography.text = biography.text + " served in the ";
                    buildItems.Clear();
                    if (reader.GetIntValue("brnAR") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbBranches["AR"]);
                        mainBranch = "AR";
                    }
                    if (reader.GetIntValue("brnAA") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbBranches["AA"]);
                        mainBranch = "AA";
                    }
                    if (reader.GetIntValue("brnAC") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbBranches["AC"]);
                        mainBranch = "AC";
                    }
                    if (reader.GetIntValue("brnAF") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbBranches["AF"]);
                        mainBranch = "AF";
                    }
                    if (reader.GetIntValue("brnAT") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbBranches["AT"]);
                        mainBranch = "AT";
                    }
                    if (reader.GetIntValue("brnCG") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbBranches["CG"]);
                        mainBranch = "CG";
                    }
                    if (reader.GetIntValue("brnMC") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbBranches["MC"]);
                        mainBranch = "MC";
                    }
                    if (reader.GetIntValue("brnNA") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbBranches["NA"]);
                        mainBranch = "NA";
                    }
                    if (reader.GetIntValue("brnNC") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbBranches["NC"]);
                        mainBranch = "NC";
                    }
                    if (reader.GetIntValue("brnWS") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbBranches["WS"]);
                        mainBranch = "WS";
                    }
                    numItems = buildItems.Count;
                    cnt = 0;
                    branches = "";
                    foreach (string item in buildItems)
                    {
                        if (cnt > 0)
                        {
                            branches += ", ";
                        }
                        if (cnt == numItems)
                        {
                            branches += "and ";
                        }
                        branches += item;
                        cnt++;
                    }
                    biography.text += branches;

                    if (reader.GetStringValue("state") != "")
                    {
                        biography.text += " from the state of " + titleCase(reader.GetStringValue("state")).Trim();
                    }
                    biography.text += ". ";

                    addPeriod = 0;
                    if (reader.GetStringValue("rank") != "")
                    {
                        string tRank = databaseControl.GetRank(mainBranch, reader.GetStringValue("rank").Trim());
                        if (tRank != "not found")
                        {
                            biography.text += sex + " attained the rank of " + titleCase(tRank).Trim();
                            addPeriod = 1;
                        }
                    }

                    if (reader.GetStringValue("unit") != "")
                    {
                        if (addPeriod == 1)
                        {
                            biography.text += " in the " + reader.GetStringValue("unit");
                        }
                        else
                        {
                            biography.text += sex + " was in the " + reader.GetStringValue("unit");
                            addPeriod = 1;
                        }
                    }
                    if (addPeriod == 1)
                    {
                        biography.text += ". ";
                    }

                    buildItems.Clear();
                    conflicts = "";
                    if (reader.GetIntValue("warIN") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbConflicts["IN"]);
                    }
                    if (reader.GetIntValue("warCV") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbConflicts["CV"]);
                    }
                    if (reader.GetIntValue("warMB") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbConflicts["MB"]);
                    }
                    if (reader.GetIntValue("warSA") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbConflicts["SA"]);
                    }
                    if (reader.GetIntValue("warWI") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbConflicts["WI"]);
                    }
                    if (reader.GetIntValue("warII") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbConflicts["II"]);
                    }
                    if (reader.GetIntValue("warKO") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbConflicts["KO"]);
                    }
                    if (reader.GetIntValue("warVN") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbConflicts["VN"]);
                    }
                    if (reader.GetIntValue("warAF") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbConflicts["AF"]);
                    }
                    if (reader.GetIntValue("warIR") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbConflicts["IR"]);
                    }
                    numItems = buildItems.Count;
                    cnt = 0;
                    foreach (string item in buildItems)
                    {
                        if (cnt > 0)
                        {
                            conflicts += ", ";
                        }
                        if (cnt == numItems)
                        {
                            conflicts += "and ";
                        }
                        conflicts += item;
                        cnt++;
                    }
                    if (numItems > 0)
                    {
                        // start the next sentence with their surname
                        biography.text += titleCase(reader.GetStringValue("lastName"));
                        biography.text += " served during " + conflicts + ".";
                    }

                    buildItems.Clear();
                    awards = "";
                    if (reader.GetIntValue("awardPH") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbAwards["PH"]);
                    }
                    if (reader.GetIntValue("awardAM") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbAwards["AM"]);
                    }
                    if (reader.GetIntValue("awardOLC") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbAwards["OLC"]);
                    }
                    if (reader.GetIntValue("awardBSM") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbAwards["BSM"]);
                    }
                    if (reader.GetIntValue("awardDSC") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbAwards["DSC"]);
                    }
                    if (reader.GetIntValue("awardLM") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbAwards["LM"]);
                    }
                    if (reader.GetIntValue("awardSS") == 1)
                    {
                        buildItems.Add((string)databaseControl.dbAwards["SS"]);
                    }
                    numItems = buildItems.Count;
                    cnt = 0;
                    foreach (string item in buildItems)
                    {
                        if (cnt > 0)
                        {
                            awards += ", ";
                        }
                        if (cnt == numItems)
                        {
                            awards += "and ";
                        }
                        awards += item;
                        cnt++;
                    }
                    if (numItems > 0)
                    {
                        // start the next sentence with their surname
                        biography.text += " For " + (sex == "He" ? "his" : "her") + " service, " + titleCase(reader.GetStringValue("lastName"));
                        biography.text += " was awarded a " + awards + ". ";
                    }

                } // end of veteran

                if (dYear > 0)
                {
                    biography.text += " " + sex + " died in " + dYear.ToString() + ".<br><br>";
                }
            } // end of bio materials
        }

        loadedBio = true;
    }

    /// <summary>
    /// Converts a string to title case.
    /// </summary>
    /// <param name="inStr">The string to convert.</param>
    /// <returns></returns>
    static string titleCase (string inStr)
    {
        string outStr = "";
        // force lower case and split on spaces
        var words = inStr.ToLower().Split(" ");

        foreach (string word in words)
        {
            if (word.Length == 0)
                outStr = outStr + "";
            else if (word.Length == 1)
            {
                outStr = outStr + word.ToUpper() + ".";
            }
            else
            {
                outStr = outStr + char.ToUpper(word[0]) + word.Substring(1) + " ";
            }
        }
        return outStr.Trim();
    }

    /// <summary>
    /// This coroutine is used to retrieve the headstone texture and set it on the game object.
    /// </summary>
    /// <remarks>
    /// The paths differ per platform.
    /// </remarks>
    /// <returns>The web request for the file.</returns>
    public IEnumerator GetHSTexture()
    {
        bool hasTexture = true;
        // get the renderer component
        _renderHS = gameObject.transform.Find("Headstone0").GetComponent<Renderer>();

        //debugamy - put up plain texture until the remainder are made
        string tFileName = "";
        string tName = gameObject.name.Trim();
        //Plain texture for missing photos
        tFileName = tName;

        // set the texture file and path
#if UNITY_EDITOR
        _strFaceFileName = "file://" + Application.dataPath + "/StreamingAssets/hsMaterials/" + tFileName + ".png";
#elif UNITY_ANDROID
        _strFaceFileName = "jar:file://" + Application.dataPath + "!/assets/hsMaterials/" + tFileName + ".png";
#endif
        // get the new texture via a web file request         
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_strFaceFileName))
        {
            // wait for the request to complete
            yield return uwr.SendWebRequest();

            // check for read errors
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("HS file: " + gameObject.name.Trim());
                Debug.Log(uwr.error);
                hasTexture = false;
            }
            else
            {
                Destroy(texHSNew);
                // read in the file for the texture
                texHSNew = DownloadHandlerTexture.GetContent(uwr);
            }
        }

        if(inRange && hasTexture)
        {
            _renderHS.material.SetTexture("_MainTex", texHSNew);
        }
    }

    public void SetTexture(Texture2D tex)
    {
        Destroy(texHSNew);
        _renderHS.material.SetTexture("_MainTex", tex);
    }

    /// <summary>
    /// This coroutine is used to retrieve the biographical material and set it on the board object.
    /// </summary>
    /// <remarks>
    /// The paths differ per platform.
    /// </remarks>
    /// <returns>The web request for the file.</returns>
    IEnumerator GetBioMaterialTexture()
    {
        // get the renderer component
        Transform rbchild = transform.Find("BillboardRotation");
        Transform bchild = rbchild.transform.Find("bioBoardBack");
        Transform bbchild = bchild.transform.Find("board");
        _renderBio = bbchild.GetComponent<Renderer>();

        // set the texture file and path
#if UNITY_EDITOR
        _strBioMatFileName = "file://" + Application.dataPath + "/StreamingAssets/hsBioMaterials/" + gameObject.name.Trim() + "-bio.png";
#elif UNITY_ANDROID
        _strBioMatFileName = "jar:file://" + Application.dataPath + "!/assets/hsBioMaterials/"  + gameObject.name.Trim() + "-bio.png";
#endif

        // get the texture via a web file request         
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_strBioMatFileName))
        {
            // wait for the request to complete
            yield return uwr.SendWebRequest();

            // check for read errors
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("HS bio material file: " + gameObject.name.Trim());
                Debug.Log(uwr.error);
            }
            else
            {
                // read in the file for the texture
                texBioMaterial = DownloadHandlerTexture.GetContent(uwr);
                // set the new texture to the main texture of the headstone
                _renderBio.material.SetTexture("_MainTex", texBioMaterial);
            }
        }
    }

    /// <summary>
    /// This coroutine is used to retrieve the branch button texture and
    /// set it on the clicker game object.
    /// </summary>
    /// <remarks>
    /// The paths differ per platform.
    /// </remarks>
    /// <returns>The web request for the file.</returns>
    public IEnumerator GetBranchTexture()
    {
        Destroy(texBranch);
        // get the identification for the headstone
        var headStone = gameObject.name.Trim();

        // build the SQL query based on the name of the headstone gameobject
        // as that is where the burial ID is stored. We want all names on the headstone.
        // bIDstr would have the section and site
        string bIDstr = headStone.Substring(0,headStone.Length - 2);

        string[] hsPieces = bIDstr.Split('-');
        if (hsPieces.Length > 2)
        {
            // need to put the A/B lettering back on
            hsPieces[1] += "-" + hsPieces[2];
        }
        var sqlStr = "SELECT brnAA, brnAR, brnAC, brnAT, brnNC, brnWS, brnMC, brnNA, brnAF, brnCG FROM cemVRburials WHERE section == '" + hsPieces[0] + "' and site == '" + hsPieces[1] + "' ORDER BY markerFace";

        // query the database for this headstone
        DBReader reader = databaseControl.db.Select(sqlStr);
        while(reader != null && reader.Read()) {
            // only set the first branch once per headstone for button material
            if (hsBranch == "")
            {
                // if this gets expanded to other cemeteries, need to cover all subsets
                if (reader.GetStringValue("brnAA") == "1" || reader.GetStringValue("brnAR") == "1" || reader.GetStringValue("brnAC") == "1" || reader.GetStringValue("brnAT") == "1" || reader.GetStringValue("brnNC") == "1" || reader.GetStringValue("brnWS") == "1")
                {
                    // set to Army
                    hsBranch = "Button-Army.png";
                }
                else if (reader.GetStringValue("brnMC") == "1")
                {
                    // set to Marine Corps
                    hsBranch = "Button-Marines.png";
                }
                else if (reader.GetStringValue("brnNA") == "1")
                {
                    // set to Navy
                    hsBranch = "Button-Navy.png";
                }
                else if (reader.GetStringValue("brnAF") == "1")
                {
                    // set to Air Force
                    hsBranch = "Button-AirForce.png";
                }
                else if (reader.GetStringValue("brnCG") == "1")
                {
                    // set to Coast Guard
                    hsBranch = "Button-CoastGuard.png";
                }
                else
                {
                    // if nothing is set, set to blank
                    hsBranch = "Button-Blank.png";
                }
                // the branch button texture file must be retrieved via a coroutine
                // after the record is read in for the detailed information
            } // end hsBranch
        }
        // set the texture file and path
#if UNITY_EDITOR
        _strBranchFileName = "file://" + Application.dataPath + "/StreamingAssets/buttonMaterials/" + hsBranch.Trim();
#elif UNITY_ANDROID
        _strBranchFileName = "jar:file://" + Application.dataPath + "!/assets/buttonMaterials/" + hsBranch.Trim();
#endif

        // get the new texture via a web file request         
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_strBranchFileName))
        {
            // wait for the request to complete
            yield return uwr.SendWebRequest();

            // check for read errors
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Branch file: " + gameObject.name.Trim());
                Debug.Log(uwr.error);
            }
            else
            {
                // read in the file for the texture
                texBranch = DownloadHandlerTexture.GetContent(uwr);
            }
        }
        // set the new texture to the main texture of the clicker
        // work down to the proper object
        Transform child = transform.Find("LogoButton0");
        Transform gchild = child.Find("Push");
        // get the renderer for the push button object
        _renderButton = gchild.GetComponent<Renderer>();
        _renderButton.material.SetTexture("_MainTex", texBranch);
    }
}
