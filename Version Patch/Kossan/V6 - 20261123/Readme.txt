Fast Track Fixed Asset - KOSSAN Version Patch V6 (23/11/2026)
==============================================================
For: KOSSAN ONLY (Kossan program, GitHub branch Kossan-V6).
Standard clients use "Version Patch\Fast Track" instead.

This patch brings all standard enhancements up to V6 into the Kossan program, while keeping
Kossan's own changes:
   - Captions Plant, Project Code, Department, Area Code, Ref. Date 1, PO No., VR No.
   - Referenced Document and Component tabs on the asset, Kossan reports and numbering fixes.
It is a big step (Kossan had none of the standard enhancements before), so test on a COPY of
the Kossan database first.


WHAT'S NEW FOR KOSSAN
---------------------
1. Run Depreciation
   - About 4x faster. Runs in the background with a progress window (assets done / total,
     errors, time left, Stop, "Run in Background") and a summary with a searchable error
     list and CSV download.
   - Filters for Plant, Project Code, Department, Cost Centre D3-D5 and Area Code / Indicators.
   - While depreciation runs, posting / cancelling documents in that month, importing assets /
     transactions and a second run are blocked with a message.
   - "Missing depreciation" check now works: an asset with a month without depreciation stops
     with "Missing depreciation on ..." until that month is run (see Step 2).
2. Posting Widget: Preview / Post filter by Plant and Project Code / Department / Cost Centre D3-D5.
3. Data Import (Excel / CSV) for F/A Class, Group, Master, Indicator, Location and all transactions,
   with "Run Simulation" and a result summary window.
   Fixed Asset Master import also takes the Referenced Documents (Kossan):
     - "Download Template" gives a .zip: "Fixed Asset Master.xlsx" (one row per asset, with an extra
       column "Import Doc No.") and "Fixed Asset Master - Referenced Documents (optional).xlsx".
     - Give each asset that has documents a key in "Import Doc No." (e.g. A1). In the Referenced
       Documents file put that key on every document row of that asset.
     - Import: asset file as "Header File", Referenced Documents file as "Lines File".
       Assets without documents: leave "Import Doc No." blank. The asset file alone still works.
4. Duplicate (Fixed Asset Master) also copies the asset's Referenced Documents.
5. Password Administration (password rules, expiry, lock after failed logons).
   Unlock: Profile > Staff > open the user > Edit > untick "Locked" > Save.
6. Confirmation popups, Crystal report viewer no longer moves right on Next Page.
7. Reset Database also clears the depreciation run log.
8. Earlier standard enhancements (V1-V5) that Kossan did not have yet, e.g. Company Setup captions
   for Cost Centres / Indicators, quantity decimals, disposal options, Status on the asset screen.
   The "Advanced" button on the asset screen is replaced by the standard layout.


HOW TO INSTALL
--------------
Do it outside office hours. Nobody should be using the Fixed Asset system.

Step 1 - Backup
   a. Back up every Kossan company FA database (FA_xxx). FA_COMMON is not changed.
   b. Copy the whole IIS Fixed Asset folder to a backup folder (program, Web.config, reports).

Step 2 - Check for missing depreciation (read-only, every company FA database)
   a. Open "Optional - Check missing depreciation before patch (read-only).sql" in SSMS.
   b. Set @Before to the first day of the next month to be depreciated (e.g. '2026-12-01').
   c. Execute on the company database. It changes nothing.
   d. Assets listed have a month without depreciation. Run depreciation for those month(s)
      first, or they will stop with "Missing depreciation on ..." after the patch.

Step 3 - Database patch (every Kossan company FA database)
   a. Open "Run this Script at your FA DB to patch.sql" in SSMS.
   b. Select the company FA database (NOT FA_COMMON, NOT the SAP database).
   c. Execute (F5). The last result row must show OK in every column (0 to 8).
   Notes:
   - Safe to run again.
   - Messages you may see on the first run (normal):
       "SBO_SP_PostTransactionNotice replaced with the standard version" - needed for the new
         audit columns. If Kossan's server copy has extra changes, merge them back from Step 1.
       "sp_FTS11_RegenerateView updated" - G/L account lists now show all postable accounts.
         Open Company Setup and Save once to refresh the account lists.
   - Part 8 sets the Company Setup captions Project Code / Department / Area Code (only where the
     caption is blank or still the standard name). Other captions can be changed in Company Setup.

Step 4 - Program files
   a. Stop the Fixed Asset site in IIS (or its Application Pool).
   b. Copy everything inside "Copy and Replace Files inside this folder to IIS Fixed Asset bin folder"
      into the site's bin folder and replace (21 files, incl. log4net.dll, Interop.SAPbobsCOM.dll,
      DevExpress.Web.Resources / Spreadsheet / Docs for the new screens).
   c. Copy everything inside "Copy and Replace Files inside this folder to IIS Fixed Asset main folder"
      into the site's main folder (with Default.aspx, Login.aspx, Web.config) and replace:
      Default.aspx, Reports.aspx, MainScreenTemplate.ascx, DeprnProgress.aspx, ImportResult.aspx,
      ImportDownload.ashx and the crystalreportviewers13 sub-folder.
   d. Report file: "FAReports (...)\fts11report_label.rpt" is the updated F/A Label report. Copy it
      only into the folder this server uses for reports (Company Setup report path), after
      backing up the old one. Kossan's own reports (F/A Listing with Components, F/A Listing -
      Details) are not in this patch - keep the ones on the server.
   e. Do NOT replace Web.config. Edit it as in Step 5.

Step 5 - Web.config (edit, do not replace; keep the server's connection string)
   a. In <configSections> <sectionGroup name="devExpress"> add:
        <section name="errors" type="DevExpress.Web.ErrorsConfigurationSection, DevExpress.Web.v19.2, Version=19.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" requirePermission="false" />
   b. In <devExpress> (after </resources>) add:
        <errors callbackErrorRedirectUrl="" />
   c. In <appSettings> add (use "https" if the site runs on https):
        <add key="HTTPSecure" value="http" />
      REQUIRED - without it Fixed Asset Reports / Posting preview cannot open the report window.
   d. In <system.webServer> <handlers>, as the FIRST line inside <handlers>, add:
        <remove name="CrystalImageHandler.aspx_GET" />
   e. In <system.web> <compilation> <assemblies> add:
        <add assembly="log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=692FBEA5521E1304" />
   f. Save and start the site / Application Pool again.

Step 6 - Test (log in to a Kossan company)
   [ ] Log in works (a "Connection Error" means Step 3 was not run on that database).
   [ ] Fixed Asset Master: open an asset > Categorization shows Plant / Project Code / Department /
       Area Code; Referenced Document and Component tabs show.
   [ ] Fixed Asset Reports > F/A Listing > Preview opens; Next Page does not move the report.
   [ ] Posting Widget > Preview (do not click Post unless the client wants to post).
   [ ] Depreciation Widget > Run Depreciation on the next month (or a test copy): progress window,
       then summary.
   [ ] Data Import > Write Off > Run Simulation with a small file.
   [ ] Fixed Asset Master > open an asset with Referenced Documents > Duplicate: the new asset's
       Referenced Document tab shows the same documents (Cancel if you do not want to keep it).


TESTED (23/11/2026, on a copy of FA_NewIQ / NewIQ_Live)
------------------------------------------------------
Log in, asset screen and tabs, captions, Depreciation Widget run (2,431 assets, 8 s), Posting Widget
preview, Data Import simulation, F/A Listing report, new Disposal form, database script twice,
Duplicate with Referenced Documents (saved, 3 of 3 copied), asset import with Referenced Documents
(simulation with errors, real import: 2 / 0 / 1 documents), template download (.zip, 2 files).
Not tested: Post to SAP, real import, Kossan's own server-only reports.


IF SOMETHING GOES WRONG
-----------------------
- Program: stop the site, copy the backed-up IIS folder back (incl. Web.config), start the site.
- Database: restore the backup from Step 1.


FILES IN THIS PATCH
-------------------
Run this Script at your FA DB to patch.sql                   - database patch (parts 0-8 in one file)
Optional - Check missing depreciation before patch (read-only).sql
Copy and Replace Files ... bin folder\                       - program DLLs (Kossan V6 build)
Copy and Replace Files ... main folder\                      - pages, crystalreportviewers13, F/A Label report
