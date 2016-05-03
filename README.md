# NUnitCompatibilityWrapper
A wrapper to accept NUnit 2 commands and transfer them to NUnit 3 commands. This wrapper was created to make sure NUnit 3 could be used in Bamboo with the NUnit runner task.

Currently it will modify following:
Change -xml and set the format to nunit2
Modify the -include and -exclude commands to a where query