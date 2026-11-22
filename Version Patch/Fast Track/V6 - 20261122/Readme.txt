Fast Track Fixed Asset - Version Patch V6 (22/11/2026)
=====================================================
For: clients on the standard Fast Track program (GitHub master branch), currently on V5.
Not for Kossan (Kossan has its own program).
Older databases (before V5, e.g. FA_NewIQ) are also covered: part 0 of the database script adds the
earlier-version tables / columns they miss. On a V5 database part 0 changes nothing.


WHAT'S NEW IN V6
----------------
1. Run Depreciation
   - Much faster (new database indexes; about 4x faster on 10,000 assets).
   - Runs in the background with a progress window: assets done / total, errors so far,
     time left, Stop button, "Run in Background". Summary at the end
     (Generated / Skipped / Errors / Time Taken) with an error list that can be
     searched, sorted and downloaded as CSV.
   - Run Depreciation filters now also work for Project, Cost Centre D1-D5 and Indicator 1-6.
   - While depreciation is running, posting / cancelling documents dated in that month,
     importing assets / transactions and a second depreciation run are blocked with a message.
   - Corrected "Missing depreciation" check (the old check never fired).
   - Reset Database also clears the depreciation run log (run history and its error list).
     Password Administration settings / history are kept, like the users and Company Setup.
2. Posting Widget
   - Preview / Post now filter by Project From/To and Cost Centre D1-D5 From/To.
3. Data Import (Excel templates for F/A Class, Group, Master, Indicator, Location and the transactions).
   - "Run Simulation" checks the file without saving anything.
   - After Import / Simulation a summary window shows (like Run Depreciation): Records Valid / Imported,
     Errors, Saved, Time Taken, and the details per row with search, sort, "Errors only" and CSV download.
4. Password Administration (password rules, expiry, lock after failed logons).
   - A locked user unlocks by itself after "Users Account Lock Duration" minutes.
   - To unlock now: Profile > Staff > open the user > Edit > untick "Locked" > Save.
     (This also resets Failed Logon Attempts.)
   - If the only administrator is locked, wait for the lock duration, or run on the company FA DB:
       UPDATE SystemUsers SET IsLocked = 0, FailedLogonCount = 0, LockedDate = NULL
       WHERE Oid = (SELECT Oid FROM PermissionPolicyUser WHERE UserName = 'the user name');
5. Confirmation boxes (Run Depreciation, Post, Delete, ...) now use the same popup
   as the Cancel button instead of the browser's box.
6. Crystal report viewer no longer moves to the right on every Next Page
   (happened when Windows display scaling was not 100%).


HOW TO INSTALL
--------------
Do it outside office hours. Nobody should be using the Fixed Asset system.

Step 1 - Backup
   a. Back up every company FA database (FA_xxx). FA_COMMON is not changed.
   b. Copy the whole IIS Fixed Asset folder (e.g. C:\inetpub\wwwroot\FixedAsset) to a backup folder.

Step 2 - Check for missing depreciation (read-only, every company FA database)
   a. Open "Optional - Check missing depreciation before patch (read-only).sql" in SSMS.
   b. Change @Before to the first day of the next month to be depreciated
      (e.g. '2026-12-01' before running December 2026).
   c. Execute on the company database. It changes nothing.
   d. If it lists assets: those assets have a month without depreciation. Discuss with the
      client and run depreciation for the missing month(s) first. After the patch, these
      assets stop with "Missing depreciation on ..." until the gap is filled.

Step 3 - Database patch (every company FA database)
   a. Open "Run this Script at your FA DB to patch.sql" in SSMS.
   b. Select the company FA database (NOT FA_COMMON, NOT the SAP database).
   c. Execute (F5). It takes a few seconds to a few minutes (index build on big databases).
   d. The last result row must show OK in every column.
   e. Repeat for each company FA database.
   Notes:
   - Safe to run again.
   - If you see "This is not a Fixed Asset company database", you picked the wrong
     database. Nothing was changed. Ignore any other errors listed after it.
   - Parts 5 and 6 replace sp_FTS11_PrepareJEPreview and sp_FTS11_GenerateDeprn with the
     standard Fast Track version. If this client has a customised version of either
     procedure, compare first (SSMS > right-click > Modify) and merge by hand.
   - Part 0 (older databases only) may also print:
       "SBO_SP_PostTransactionNotice replaced with the standard version" - its old
         FTS11FAM2 insert would fail after the new columns. If this client had other changes in
         that procedure, merge them back from the Step 1 backup.
       "sp_FTS11_RegenerateView updated" - the G/L account lists now show all postable accounts
         (not only Level 5). Open Company Setup and Save once to refresh the account lists.
     Part 0 does not touch client-specific objects (report functions, report grouping captions,
     extra tables).

Step 4 - Program files
   a. Stop the Fixed Asset site in IIS (or stop its Application Pool).
   b. Copy everything inside
      "Copy and Replace Files inside this folder to IIS Fixed Asset bin folder"
      into the site's bin folder and replace.
   c. Copy everything inside
      "Copy and Replace Files inside this folder to IIS Fixed Asset main folder"
      into the site's main folder (the folder with Default.aspx, Login.aspx, Web.config) and replace.
   d. Do NOT replace Web.config. V6 needs no Web.config change.
   e. Start the site / Application Pool again.

Step 5 - Test (log in to a company)
   [ ] Log in works.
   [ ] Fixed Asset Reports > Preview a report > click Next Page a few times: the report does not move.
   [ ] Posting Widget > set a Cost Centre D1 From/To > Preview: only that cost centre is shown.
       (Do not click Post unless the client wants to post.)
   [ ] Depreciation Widget > Run Depreciation on the next month (or a test company):
       the progress window shows, then the summary.
   [ ] Data Import screen opens and template download works.


KNOWN ISSUE (not caused by V6, same as before the patch)
-----------
These Fixed Asset Reports may show "The report you requested requires further information"
(a database log-on box) instead of the report:
   F/A Ledger, F/A Transaction - Acquisition, F/A Transaction - Disposal,
   F/A Transaction - Review Useful Life
These reports were saved in Crystal with the old "SQL Server Native Client 10" driver (SQLNCLI10), which is
not installed on newer servers.
Quick fix (tested, all 4 reports then open): install SQL Server 2008 R2 Native Client x64 on the IIS server.
   - Download from Microsoft: https://www.microsoft.com/en-us/download/details.aspx?id=57606
     pick "2008 R2 ENU-1033 / x64 / sqlncli.msi" (7.8 MB, version 10.50.6560, TLS 1.2 capable).
   - Run it as administrator, accept the licence, finish. No restart of Windows or IIS needed.
   - Check: Control Panel > Programs shows "Microsoft SQL Server 2008 R2 Native Client 10.53.6560.0".
Permanent fix: open each .rpt in Crystal Reports > Database > Set Datasource Location > point it to the FA
database with a current OLE DB driver (MSOLEDBSQL) > save.
Do not type the password into the log-on box for users.


IF SOMETHING GOES WRONG
-----------------------
- Program: stop the site, copy the backed-up IIS folder back, start the site.
- Database: restore the backup from Step 1. The new indexes and tables are harmless with
  the old program; the two procedures (parts 5 and 6) can be restored from the backup.


FILES IN THIS PATCH
-------------------
Run this Script at your FA DB to patch.sql                   - database patch (parts 0-6 in one file)
Optional - Check missing depreciation before patch (read-only).sql
Copy and Replace Files ... bin folder\                       - program DLLs (FT_FixedAsset.*, FileSystemData,
                                                               DevExpress Spreadsheet / Docs for Data Import)
Copy and Replace Files ... main folder\                      - Reports.aspx, DeprnProgress.aspx, ImportDownload.ashx, ImportResult.aspx
