# in-memorykeyvalue

This demo application is coded with extra feature in the debug mode, 
like displaying all entries in the key-value database by choosing get and pushing enter twice 
and put will allow multiple entries without having to type put with each entry.

Found some things in the requirements that conflicted in my mind.

1)Termination operations seems to conflict with do not write to disk or I just didn't understand what was asked.
2)dirty read not specified so the get just reports what is in the commited database.
3)not sure if you wanted to actually use redis or Couchdb or some other key-value system so 
  I just created a pseudo key-value system with C# available features.
