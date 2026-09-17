INSERT INTO "SysProcessUserTask" ("SysUserTaskSchemaUId", "Caption")
SELECT s."UId", 'Arithmetic' FROM "SysSchema" s
WHERE s."UId" = 'd3ad1be6-6c0f-4c80-a4a4-5f1f1e2df7ea'
AND NOT EXISTS (SELECT 1 FROM "SysProcessUserTask" t WHERE t."SysUserTaskSchemaUId" = s."UId");

INSERT INTO "SysProcessUserTask" ("SysUserTaskSchemaUId", "Caption")
SELECT s."UId", 'Arithmetic: Subtract' FROM "SysSchema" s
WHERE s."UId" = 'caae8936-c70a-4023-8a61-837b1d5c57fc'
AND NOT EXISTS (SELECT 1 FROM "SysProcessUserTask" t WHERE t."SysUserTaskSchemaUId" = s."UId");

INSERT INTO "SysProcessUserTask" ("SysUserTaskSchemaUId", "Caption")
SELECT s."UId", 'Arithmetic: Multiply' FROM "SysSchema" s
WHERE s."UId" = '90b35928-3384-482c-800a-49bc0aee4532'
AND NOT EXISTS (SELECT 1 FROM "SysProcessUserTask" t WHERE t."SysUserTaskSchemaUId" = s."UId");

INSERT INTO "SysProcessUserTask" ("SysUserTaskSchemaUId", "Caption")
SELECT s."UId", 'Arithmetic: Divide' FROM "SysSchema" s
WHERE s."UId" = '07fbe6d8-5088-403a-a9f6-2a4320dc8f9c'
AND NOT EXISTS (SELECT 1 FROM "SysProcessUserTask" t WHERE t."SysUserTaskSchemaUId" = s."UId");
