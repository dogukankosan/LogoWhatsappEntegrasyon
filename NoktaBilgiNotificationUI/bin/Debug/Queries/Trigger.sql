ALTER TRIGGER [dbo].[NKT_ORFICHE_WP_TRIGGER] 
ON [dbo].[LG_001_01_ORFICHE]
AFTER INSERT, DELETE, UPDATE
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION
        SELECT * INTO #InsertedTemp FROM inserted

        IF EXISTS (
            SELECT 1
            FROM #InsertedTemp ORFFICHE
            WHERE ORFFICHE.CLIENTREF <> 0 AND ORFFICHE.NETTOTAL > 0 AND
                  EXISTS (
                      SELECT 1 
                      FROM NKT_TABLEFILTER NKT_FILTER WITH (NOLOCK)
					  /*f*/
                      WHERE
                          NKT_FILTER.CustomerLogicalRef = ORFFICHE.CLIENTREF AND 
                          NKT_FILTER.SalesmanLogicalRef = ORFFICHE.SALESMANREF AND
                          (NKT_FILTER.OrderMax <= ORFFICHE.NETTOTAL AND NKT_FILTER.CustomerLogicalRef = ORFFICHE.CLIENTREF) AND
                          NKT_FILTER.PayDay = ORFFICHE.PAYDEFREF
					/*d*/
                  )
        )
        BEGIN
            IF EXISTS (
                SELECT 1 
                FROM NKT_ORFFICHELOGICALREFWP LOGIC WITH (NOLOCK) 
                JOIN #InsertedTemp T ON LOGIC.FICHELOGICALREF = T.LOGICALREF
                WHERE ISNULL(LOGIC.FICHENO, '') <> ISNULL(T.FICHENO, '')
            )
            BEGIN
                UPDATE LOGIC
                SET LOGIC.FICHENO = T.FICHENO
                FROM NKT_ORFFICHELOGICALREFWP LOGIC WITH (NOLOCK)
                JOIN #InsertedTemp T ON LOGIC.FICHELOGICALREF = T.LOGICALREF
            END

            IF EXISTS (SELECT 1 FROM #InsertedTemp WHERE TRCODE = 1 AND STATUS = 1)
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM deleted)
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 
                        FROM NKT_ORFFICHELOGICALREFWP W WITH (NOLOCK)
                        JOIN #InsertedTemp I ON W.FICHELOGICALREF = I.LOGICALREF
                    )
                    BEGIN
                        INSERT INTO NKT_ORFFICHELOGICALREFWP (
                            FICHELOGICALREF,
                            FICHENO,
                            ACTIVESTATUS
                        )
                        SELECT 
                            LOGICALREF,
                            FICHENO,
                            0
                        FROM #InsertedTemp
                    END
                END
                ELSE
                BEGIN
INSERT INTO NKT_ORFFICHELOGICALREFWP (
    FICHELOGICALREF,
    FICHENO,
    ACTIVESTATUS
)
SELECT 
    ORFFICHE.LOGICALREF,
    ORFFICHE.FICHENO,
    0
FROM #InsertedTemp ORFFICHE
WHERE ORFFICHE.CLIENTREF <> 0 AND ORFFICHE.NETTOTAL > 0 AND
      NOT EXISTS (
          SELECT 1 FROM NKT_ORFFICHELOGICALREFWP W
          WHERE W.FICHELOGICALREF = ORFFICHE.LOGICALREF
      )
      AND EXISTS (
          SELECT 1 
          FROM NKT_TABLEFILTER NKT_FILTER WITH (NOLOCK)
		  /*f*/
          WHERE
              NKT_FILTER.CustomerLogicalRef = ORFFICHE.CLIENTREF AND 
              NKT_FILTER.SalesmanLogicalRef = ORFFICHE.SALESMANREF AND
              (NKT_FILTER.OrderMax <= ORFFICHE.NETTOTAL AND NKT_FILTER.CustomerLogicalRef = ORFFICHE.CLIENTREF) AND
              NKT_FILTER.PayDay = ORFFICHE.PAYDEFREF
		  /*d*/
      )
                END
            END
        END

        DROP TABLE #InsertedTemp

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION
        DECLARE @ErrMsg NVARCHAR(4000), @ErrSeverity INT
        SELECT 
            @ErrMsg = ERROR_MESSAGE(),
            @ErrSeverity = ERROR_SEVERITY()
        RAISERROR(@ErrMsg, @ErrSeverity, 1)
    END CATCH
END;