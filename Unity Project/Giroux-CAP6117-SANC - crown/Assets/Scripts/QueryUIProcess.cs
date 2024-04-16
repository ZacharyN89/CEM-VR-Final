using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// This class controls the UI processes.
/// </summary>
public class QueryUIProcess : MonoBehaviour
{
    /// <summary>
    /// Holds a reference to the database control script.
    /// </summary>
    public DatabaseControl databaseControl;

    /// <summary>
    /// Holds a reference to the basic query item dropdown.
    /// </summary>
    public TMP_Dropdown itemDDB;
    /// <summary>
    /// Holds a reference to the basic query operator dropdown.
    /// </summary>
    public TMP_Dropdown operDDB;
    /// <summary>
    /// Holds a reference to the basic query value dropdown.
    /// </summary>
    public TMP_Dropdown valueDDB;

    /// <summary>
    /// Holds a reference to the array of field dropdowns.
    /// </summary>
    public TMP_Dropdown[] fieldList;
    /// <summary>
    /// Holds a reference to the array of operator dropdowns.
    /// </summary>
    public TMP_Dropdown[] operList;
    /// <summary>
    /// Holds a reference to the array of value dropdowns.
    /// </summary>
    public TMP_Dropdown[] valueList;
    /// <summary>
    /// Holds a reference to the array of value dropdowns.
    /// </summary>
    public TMP_InputField[] inputList;
    /// <summary>
    /// Holds a reference to the array of AND/OR dropdowns.
    /// </summary>
    public TMP_Dropdown[] andOrs;
    /// <summary>
    /// Holds a reference to the array of left parentheses.
    /// </summary>
    public Toggle[] leftParen;
    /// <summary>
    /// Holds a reference to the array of right parentheses.
    /// </summary>
    public Toggle[] rightParen;
    /// <summary>
    /// Holds a reference to the array of delete buttons.
    /// </summary>
    public Button[] delButtons;
    /// <summary>
    /// Holds a reference to the array of active flags for the advanced rows.
    /// </summary>
    public bool[] activeRows;
    /// <summary>
    /// Holds a reference to the array of the row game objects for the advanced query.
    /// </summary>
    public GameObject[] advRows;
    /// <summary>
    /// Holds a reference to the submit bottom of the advanced query screen.
    /// </summary>
    public Button submitAdv;

    List<string> fieldsBasic;
    List<string> fieldsAdvanced;
    List<string> operNormal;
    List<string> operDate;
    List<string> operName;

    private int cntActiveRows;

    /// <summary>
    /// Initializes the advanced query dropdown lists.
    /// </summary>
    private void Awake()
    {
        // initialize the lists
        fieldsBasic = new List<string>();
        fieldsAdvanced = new List<string>();
        operNormal = new List<string>();
        operDate = new List<string>();
        operName = new List<string>();
        // set the count of active rows to 1
        cntActiveRows = 1;
    }

    /// <summary>
    /// Sets the values for the default dropdowns.
    /// </summary>
    void Start()
    {
        // set up the basic tab
        initializeBasic();

        // set up the advanced tab
        initializeAdvanced();
    }

    /// <summary>
    /// Initializes the fields on the advanced query tab.
    /// </summary>
    void initializeAdvanced()
    {
        // clear all the dropdowns and input fields except for the field items
        for (int i = 0; i <= 4; i++)
        {
            if (operList[i] != null)
            {
                operList[i].options.Clear();
            }
            if (valueList[i] != null)
            {
                valueList[i].options.Clear();
            }
            activeRows[i] = false;
        }

        // define the choices for normal operator dropdowns
        operNormal.Add("");
        operNormal.Add("Equals");
        operNormal.Add("Does not equal");

        // define choices for date dropdowns
        operDate.Add("");
        operDate.Add("Equals");
        operDate.Add("Does not equal");
        operDate.Add("Greater than");
        operDate.Add("Less than");
        operDate.Add("Greater or equal");
        operDate.Add("Less or equal");

        // define choices for names
        operName.Add("");
        operName.Add("Equals");
        operName.Add("Does not equal");
        operName.Add("Starts with");
        operName.Add("Contains");

        // clear the fields dropdown before filling it
        fieldsAdvanced.Clear();
        // get the common fields
        commonFields(fieldsAdvanced);
        // add the advanced specific fields
        fieldsAdvanced.Add("Last Name");
        fieldsAdvanced.Add("First Name");
        fieldsAdvanced.Add("Middle Name");
        fieldsAdvanced.Add("Suffix");
        fieldsAdvanced.Add("Birth Year");
        fieldsAdvanced.Add("Death Year");
        // add a blank option and sort them 
        fieldsAdvanced.Add("");
        fieldsAdvanced.Sort();

        // add the list to the 5 rows dropdown
        for (int i = 0; i <= 4; i++)
        {
            // clear previous
            fieldList[i].options.Clear();
            // add new
            foreach (var fld in fieldsAdvanced)
            {
                fieldList[i].options.Add(new TMP_Dropdown.OptionData() { text = fld });
            }
        }
        // default to the blank selection
        fieldList[0].value = 0;
        FieldList0Selected(fieldList[0]);
        // add the listener to the dropdown
        fieldList[0].onValueChanged.AddListener(delegate
        {
            FieldList0Selected(fieldList[0]);
        }); 
        // default to the blank selection
        fieldList[1].value = 0;
        FieldList1Selected(fieldList[1]);
        // add the listener to the dropdown
        fieldList[1].onValueChanged.AddListener(delegate
        {
            FieldList1Selected(fieldList[1]);
        });
        // default to the blank selection
        fieldList[2].value = 0;
        FieldList2Selected(fieldList[2]);
        // add the listener to the dropdown
        fieldList[2].onValueChanged.AddListener(delegate
        {
            FieldList2Selected(fieldList[2]);
        });
        // default to the blank selection
        fieldList[3].value = 0;
        FieldList3Selected(fieldList[3]);
        // add the listener to the dropdown
        fieldList[3].onValueChanged.AddListener(delegate
        {
            FieldList3Selected(fieldList[3]);
        });
        // default to the blank selection
        fieldList[4].value = 0;
        FieldList4Selected(fieldList[4]);
        // add the listener to the dropdown
        fieldList[4].onValueChanged.AddListener(delegate
        {
            FieldList4Selected(fieldList[4]);
        });

        // set the first row to active
        activeRows[0] = true;
    }

    /// <summary>
    /// Resets the proper row variables on the Reset button for Advanced Query.
    /// </summary>
    public void ResetRows()
    {
        // process all 5 rows
        for (int i = 0; i <= 4; i++)
        {
            // only activate the first rown
            if (i == 0)
            {
                activeRows[i] = true;
                
            }
            else
            {
                // set the remaining rows to inactive
                activeRows[i] = false;
                advRows[i].SetActive(false);
            }
            // reset the count of active rows to 1
            cntActiveRows = 1;
        }
    }

    /// <summary>
    /// Sets the proper operators into the choice list.
    /// </summary>
    /// <param name="row">The screen row being adjusted.</param>
    /// <param name="fld">The field to determine the proper options.</param>
    void SetOperators(int row, string fld)
    {
        // get the last 4 characters of the field name
        string t4 = "";
        if (fld.Length - 4 > 0)
        {
            t4 = fld.Substring(fld.Length - 4);
        }

        // clear the previous values
        operList[row].ClearOptions();

        
        // on date fields 
        if (t4 == "Year")
        {
            // add the date operators to the dropdown
            foreach (var oper in operDate)
            {
                operList[row].options.Add(new TMP_Dropdown.OptionData() { text = oper });
            }
        }
        else if (t4 == "Name")
        {
            // add the name operators to the dropdown
            foreach (var oper in operName)
            {
                operList[row].options.Add(new TMP_Dropdown.OptionData() { text = oper });
            }
        }
        else
        {
            // all other fields get the normal choices of equal/does not equal
            foreach (var oper in operNormal)
            {
                operList[row].options.Add(new TMP_Dropdown.OptionData() { text = oper });
            }
        }
    }

    /// <summary>
    /// Processes the first query row when the field list changes.
    /// </summary>
    /// <param name="dropdown">The dropdown that changed.</param>
    void FieldList0Selected(TMP_Dropdown dropdown)
    {
        // Populate the operator dropdown for the chosen field
        SetOperators(0, dropdown.options[dropdown.value].text);
        // fill the value field also
        FillAdvancedRow(0);
    }

    /// <summary>
    /// Processes the second query row when the field list changes.
    /// </summary>
    /// <param name="dropdown">The dropdown that changed.</param>
    void FieldList1Selected(TMP_Dropdown dropdown)
    {
       // Populate the operator dropdown for the chosen field
       SetOperators(1, dropdown.options[dropdown.value].text);
        // fill the value field also
        FillAdvancedRow(1);
    }

    /// <summary>
    /// Processes the third query row when the field list changes.
    /// </summary>
    /// <param name="dropdown">The dropdown that changed.</param>
    void FieldList2Selected(TMP_Dropdown dropdown)
    {
        // Populate the operator dropdown for the chosen field
        SetOperators(2, dropdown.options[dropdown.value].text);
        // fill the value field also
        FillAdvancedRow(2);
    }

    /// <summary>
    /// Processes the fourth query row when the field list changes.
    /// </summary>
    /// <param name="dropdown">The dropdown that changed.</param>
    void FieldList3Selected(TMP_Dropdown dropdown)
    {
        // Populate the operator dropdown for the chosen field
        SetOperators(3, dropdown.options[dropdown.value].text);
        // fill the value field also
        FillAdvancedRow(3);
    }

    /// <summary>
    /// Processes the fifth query row when the field list changes.
    /// </summary>
    /// <param name="dropdown">The dropdown that changed.</param>
    void FieldList4Selected(TMP_Dropdown dropdown)
    {
        // Populate the operator dropdown for the chosen field
        SetOperators(4, dropdown.options[dropdown.value].text);
        // fill the value field also
        FillAdvancedRow(4);
    }


    /// <summary>
    /// Sets list of common field names for basic and advanced dropdowns
    /// </summary>
    void commonFields(List<string> whichList)
    {
        whichList.Add("Branch");
        whichList.Add("Conflict");
        whichList.Add("Rank");
        whichList.Add("Unit");
        whichList.Add("State");
        whichList.Add("Award");
        whichList.Add("Emblem of Belief");
        whichList.Add("Section");
    }

    /// <summary>
    /// Initializes the fields on the basic query tab
    /// </summary>
    void initializeBasic()
    { 
        // clear the three dropdowns
        itemDDB.options.Clear();
        operDDB.options.Clear();
        valueDDB.options.Clear();

        // fill basic query fields for itemDD
        commonFields(fieldsBasic);

        // add the list to the dropdown
        foreach (var fld in fieldsBasic)
        {
            itemDDB.options.Add(new TMP_Dropdown.OptionData() { text = fld });
        }
        itemDDB.value = 0;
        itemDDBSelected(itemDDB);
        itemDDB.onValueChanged.AddListener(delegate
        {
            itemDDBSelected(itemDDB);
        });

        // define the list of options for the operators
        List<string> opers = new List<string>();
        opers.Add("Equals");
        opers.Add("Does not equal");

        // add the list to the dropdown
        foreach (var oper in opers)
        {
            operDDB.options.Add(new TMP_Dropdown.OptionData() { text = oper });
        }
        operDDB.value = 0;
        operDDBSelected(operDDB);
        operDDB.onValueChanged.AddListener(delegate
        {
            operDDBSelected(operDDB);
        });

        // fill the values dropdown with branch as default
        FillBranch(valueDDB);
        valueDDB.value = 0;
        valueDDBSelected(operDDB);
        valueDDB.onValueChanged.AddListener(delegate
        {
            valueDDBSelected(valueDDB);
        });
        databaseControl.ResetPointers();
    }

    /// <summary>
    /// Process the selection for the item field on the basic query tab.
    /// </summary>
    /// <param name="dropdown">The item dropdown reference.</param>
    void itemDDBSelected(TMP_Dropdown dropdown)
    {
        // clear the previous dropdown values
        valueDDB.options.Clear();

        // fill the value dropdown with the proper values
        int index = dropdown.value;
        switch (dropdown.options[index].text)
        {
            case "Section":
                FillSection(valueDDB);
                break;
            case "Branch":
                FillBranch(valueDDB);
                break;
            case "Conflict":
                FillConflicts(valueDDB);
                break;
            case "Rank":
                FillRank(valueDDB);
                break;
            case "Unit":
                FillUnit(valueDDB);
                break;
            case "State":
                FillState(valueDDB);
                break;
            case "Award":
                FillAwards(valueDDB);
                break;
            case "Emblem of Belief":
                FillEmblems(valueDDB);
                break;
        }
        // reset the second two dropdowns
        operDDB.value = 0;
        operDDB.RefreshShownValue();
        valueDDB.value = 0;
        valueDDB.RefreshShownValue();
    }

    /// <summary>
    /// Process the operator dropdown.
    /// </summary>
    /// <param name="dropdown">The reference to the operator dropdown.</param>
    void operDDBSelected(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;
        // future use
    }

    /// <summary>
    /// Process the value dropdown.
    /// </summary>
    /// <param name="dropdown">The reference to the value dropdown.</param>
    void valueDDBSelected(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;
        // future use
    }


    /// <summary>
    /// Process the selection for the item field on the basic query tab.
    /// </summary>
    /// <param name="dropdown">The item dropdown reference.</param>
    void FillAdvancedRow(int row)
    {
        // clear the previous dropdown values
        valueList[row].options.Clear();
        valueList[row].value = 0;
        valueList[row].RefreshShownValue();
        // make sure the dropdown is showing by default
        valueList[row].gameObject.SetActive(true);
        inputList[row].gameObject.SetActive(false);

        // fill the value dropdown with the proper values or turn the input field on
        string fld = fieldList[row].options[fieldList[row].value].text;
        switch (fld)
        {
            case "Last Name":
            case "First Name":
            case "Middle Name":
            case "Birth Year":
            case "Death Year":
                // hide the dropdown and show the input field
                valueList[row].gameObject.SetActive(false);
                inputList[row].gameObject.SetActive(true);
                break;
            case "Suffix":
                FillSuffixes(valueList[row]);
                break;
            case "Section":
                FillSection(valueList[row]);
                break;
            case "Branch":
                FillBranch(valueList[row]);
                break;
            case "Conflict":
                FillConflicts(valueList[row]);
                break;
            case "Rank":
                FillRank(valueList[row]);
                break;
            case "Unit":
                FillUnit(valueList[row]);
                break;
            case "State":
                FillState(valueList[row]);
                break;
            case "Award":
                FillAwards(valueList[row]);
                break;
            case "Emblem of Belief":
                FillEmblems(valueList[row]);
                break;
        }
        // reset the second two dropdowns
        operList[row].value = 0;
        operList[row].RefreshShownValue();
        valueList[row].value = 0;
        valueList[row].RefreshShownValue();
    }



    /// <summary>
    /// Fill the value dropdown with branch values.
    /// </summary>
    /// <param name="whichDD">The dropdown to fill with the branches.</param>
    void FillBranch(TMP_Dropdown whichDD)
    {
        List<string> curBrn = new List<string>();
        curBrn.Add("AR");
        curBrn.Add("AA");
        curBrn.Add("AC");
        curBrn.Add("AF");
        curBrn.Add("AT");
        curBrn.Add("CG");
        curBrn.Add("MC");
        curBrn.Add("NA");
        curBrn.Add("NC");
        curBrn.Add("WS");

        // hashtables can't be sorted in place
        List<string> ht = new List<string>();
        // need to add a blank option
        ht.Add("");

        // add the branches to a list
        foreach (DictionaryEntry item in databaseControl.dbBranches)
        {
            // only add the applicable branches for now
            if (curBrn.Contains((string)item.Key))
            {
                ht.Add((string)item.Value);
            }
        }
        // sort the list
        ht.Sort();

        // clear the previous values before reloading the list
        whichDD.options.Clear();
        whichDD.value = 0;
        whichDD.RefreshShownValue();

        // loop through the newly sorted list and add to the dropdown
        foreach (string tStr in ht)
        {
            whichDD.options.Add(new TMP_Dropdown.OptionData() { text = tStr });
        }
    }

    /// <summary>
    /// Fill the value dropdown with award values.
    /// </summary>
    /// <param name="whichDD">The dropdown to fill with awards.</param>
    void FillAwards(TMP_Dropdown whichDD)
    {
        List<string> curAwd = new List<string>();
        curAwd.Add("BSM");
        curAwd.Add("DSC");
        curAwd.Add("PH");
        curAwd.Add("AM");
        curAwd.Add("LM");
        curAwd.Add("SS");

        // hashtables can't be sorted in place
        List<string> ht = new List<string>();
        // do not add a blank option as it is currently
        // impossible to test for blank as award spans multiple fields
        // add the awards to a list
        foreach (DictionaryEntry item in databaseControl.dbAwards)
        {
            // only add the applicable branches for now
            if (curAwd.Contains((string)item.Key))
            {
                ht.Add((string)item.Value);
            }
        }
        // sort the list
        ht.Sort();

        // clear the previous values before reloading the list
        whichDD.options.Clear();
        whichDD.value = 0;
        whichDD.RefreshShownValue();

        // loop through the newly sorted list and add to the dropdown
        foreach (string tStr in ht)
        {
            whichDD.options.Add(new TMP_Dropdown.OptionData() { text = tStr });
        }
    }

    /// <summary>
    /// Fill the values dropdown with conflicts.
    /// </summary>
    /// <param name="whichDD">The dropdown to fill with awards.</param>
    void FillConflicts(TMP_Dropdown whichDD)
    {
        List<string> curWar = new List<string>();
        curWar.Add("IN");
        curWar.Add("CV");
        curWar.Add("MB");
        curWar.Add("SA");
        curWar.Add("WI");
        curWar.Add("II");
        curWar.Add("KO");
        curWar.Add("VN");
        curWar.Add("AF");
        curWar.Add("IR");

        // hashtables can't be sorted in place
        List<string> ht = new List<string>();
        // do not add a blank option as it is currently
        // impossible to test for blank as conflict spans multiple fields
        // add the wars to a list
        foreach (DictionaryEntry item in databaseControl.dbConflicts)
        {
            // only add the applicable branches for now
            if (curWar.Contains((string)item.Key))
            {
                ht.Add((string)item.Value);
            }
        }
        // sort the list
        ht.Sort();

        // clear the previous values before reloading the list
        whichDD.options.Clear();
        whichDD.value = 0;
        whichDD.RefreshShownValue();

        // loop through the newly sorted list and add to the dropdown
        foreach (string tStr in ht)
        {
            whichDD.options.Add(new TMP_Dropdown.OptionData() { text = tStr });
        }
    }

    /// <summary>
    /// Fill the advanced query values dropdown with suffixes.
    /// </summary>
    /// <param name="whichDD">The dropdown to fill with suffixes.</param>
    void FillSuffixes(TMP_Dropdown whichDD)
    {
        // loop through the sorted list and add to the dropdown
        foreach (string tStr in databaseControl.dbSuffix)
        {
            whichDD.options.Add(new TMP_Dropdown.OptionData() { text = tStr });
        }
    }

    /// <summary>
    /// Fill the values dropdown with sections.
    /// </summary>
    /// <param name="whichDD">The dropdown to fill with awards.</param>
    void FillSection(TMP_Dropdown whichDD)
    {
        // loop through the sorted list and add to the dropdown
        foreach (string tStr in databaseControl.dbSection)
        {
            whichDD.options.Add(new TMP_Dropdown.OptionData() { text = tStr });
        }
    }

    /// <summary>
    /// Fill the values dropdown with emblems of belief.
    /// </summary>
    /// <param name="whichDD">The dropdown to fill with awards.</param>
    void FillEmblems(TMP_Dropdown whichDD)
    {
        // loop through the sorted list and add to the dropdown
        foreach (string tStr in databaseControl.dbEmblems)
        {
            whichDD.options.Add(new TMP_Dropdown.OptionData() { text = tStr });
        }
    }

    /// <summary>
    ///  Fill the values dropdown with all ranks.
    /// </summary>
    /// <param name="whichDD">The dropdown to fill with awards.</param>
    void FillRank(TMP_Dropdown whichDD)
    {
        // loop through the sorted list and add to the dropdown
        foreach (string tStr in databaseControl.dbAllRanks)
        {
            whichDD.options.Add(new TMP_Dropdown.OptionData() { text = tStr });
        }
    }

    /// <summary>
    /// Fill the values dropdown with units.
    /// </summary>
    /// <param name="whichDD">The dropdown to fill with awards.</param>
    void FillUnit(TMP_Dropdown whichDD)
    {
        // loop through the sorted list and add to the dropdown
        foreach (string tStr in databaseControl.dbUnit)
        {
            whichDD.options.Add(new TMP_Dropdown.OptionData() { text = tStr });
        }
    }

    /// <summary>
    /// Fill the values dropdown with states.
    /// </summary>
    /// <param name="whichDD">The dropdown to fill with awards.</param>
    void FillState(TMP_Dropdown whichDD)
    {
        // loop through the sorted list and add to the dropdown
        foreach (string tStr in databaseControl.dbState)
        {
            whichDD.options.Add(new TMP_Dropdown.OptionData() { text = tStr });
        }
    }

    /// <summary>
    /// Controls the visibility of the rows in the advanced query.
    /// </summary>
    /// <param name="row">Index to the active row.</param>
    public void UpdateRowAndOr (int row)
    {
        TMP_Dropdown tDD = andOrs[row];
        int nextRow = row + 1;

        // 0 blank, 1 AND, 2 OR
        if (tDD.value != 0)
        {
            // user has selected AND or OR
            advRows[nextRow].SetActive(true);
            // only increment the count if this was a new row
            if (activeRows[nextRow] == false)
            {
                cntActiveRows++;
                activeRows[nextRow] = true;
            }

            // and then deactive the delete button on the previous row
            // only allow to delete from the bottom up
            if (nextRow >= 2)
            {
                // turn delete off on the incoming row
                delButtons[row].enabled = false;
            }
            // turn delete on on the newly inserted row
            delButtons[nextRow].enabled = true;
        }
    }

    /// <summary>
    /// Controls the accessibility of the delete row buttons in the advanced query.
    /// </summary>
    /// <param name="row">One-based index to the active row.</param>
    public void UpdateDelButton(int row)
    {
        int prevRow = row - 1;
        
        // the delete buttons are only on rows 1-4
        // when delete is clicked, hide the row being clicked
        // activate the delete for the previous row
        // set the previous row's AndOr to blank

        // clear the and/or before hiding row and row 4 doesn't have an andor.
        if (row != 4)
        {
            andOrs[row].value = 0;
            andOrs[row].RefreshShownValue();
        }
        // row 0 doesn't have a delete button so don't try to enable it
        if (row != 1)
        {
            delButtons[prevRow].enabled = true;
        }
        // clear the previous row's and/or
        andOrs[prevRow].value = 0;
        andOrs[prevRow].RefreshShownValue();

        // hide the deleted row
        advRows[row].SetActive(false);
        fieldList[row].value = 0;
        fieldList[row].RefreshShownValue();
        operList[row].value = 0;
        operList[row].RefreshShownValue();
        // need to deactive the input fields and activate dropdowns
        inputList[row].text = "";
        inputList[row].gameObject.SetActive(false);

        valueList[row].value = 0;
        valueList[row].RefreshShownValue();
        leftParen[row].isOn = false;
        rightParen[row].isOn = false;

        // deactivate the row
        activeRows[row] = false;
        cntActiveRows--;
    }

    /// <summary>
    /// Checks the validity of the screen to enable or disable the Submit button.
    /// </summary>
    public void CheckValidity()
    {
        bool isValid = true;
        int cntAndor = 0;
        int cntLeft = 0;
        int cntRight = 0;

        // count the active AND/OR, active is 1-based
        for (int i = 0; i < cntActiveRows - 1; i++)
        {
            if (andOrs[i].value != 0)
            {
                cntAndor++;
            }
        }

        int mathCnt = cntActiveRows - 1;
        // see if the count of AND/OR is valid for the number of rows
        if (mathCnt != cntAndor)
        {
            isValid = false;
        }

        if (isValid)
        {
            // cntActiveRows is not zero-based so check for < only
            for (int i = 0; i < cntActiveRows; i++)
            {
                // unless both the field and operator are selected, this is an invalid line
                if (fieldList[i].value == 0 || operList[i].value == 0)
                {
                    isValid = false;
                    break;
                }
            }
        }

        // check parentheses
        for (int i = 0; i <= 4; i++)
        {
            if (leftParen[i].isOn)
            {
                cntLeft++;
            }
            if (rightParen[i].isOn)
            {
                cntRight++;
            }
            if (cntRight > cntLeft)
            {
                // immediate error
                isValid = false;
                break;
            }
        }
        if (cntLeft != cntRight)
        {
            // not paired
            isValid = false;
        }

        if (isValid)
        {
            submitAdv.enabled = true;
        }
        else
        {
            submitAdv.enabled = false;
        }
    }

    /// <summary>
    /// Disable all editable fields while keyboard is open.
    /// </summary>
    ///  /// <param name="tif">tif holds a reference to the currently active text input field.</param>
    public void SetAdvFields(TMP_InputField tif, bool isOn)
    {
        // get the current row number from the name of the input field
        int numOn = Convert.ToInt32(tif.name.Substring(tif.name.Length - 1));

        // loop through the query rows
        for (int i = 0; i <= 4; i++)
        {
            // if the row is active, disable the fields
            if (activeRows[i] == true)
            {
                leftParen[i].enabled = isOn;
                rightParen[i].enabled = isOn;
                fieldList[i].enabled = isOn;
                operList[i].enabled = isOn;
                valueList[i].enabled = isOn;
                andOrs[i].enabled = isOn;
                if (i != numOn)
                {
                    inputList[i].enabled = isOn;
                }
            }
            else
            {
                // hit an inactive row, so exit
                break;
            }
        }
    }
}
