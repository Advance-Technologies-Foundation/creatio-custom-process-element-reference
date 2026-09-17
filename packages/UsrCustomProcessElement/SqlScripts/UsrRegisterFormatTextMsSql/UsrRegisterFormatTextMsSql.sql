-- After-package registration; safe to execute again during an upgrade.
INSERT INTO [SysProcessUserTask] ([SysUserTaskSchemaUId], [Caption])
SELECT s.[UId], 'Format text'
FROM [SysSchema] s
WHERE s.[UId] = '1b597245-7f00-41f4-8943-8d1800a8138c'
  AND NOT EXISTS (SELECT 1 FROM [SysProcessUserTask] t WHERE t.[SysUserTaskSchemaUId] = s.[UId]);
