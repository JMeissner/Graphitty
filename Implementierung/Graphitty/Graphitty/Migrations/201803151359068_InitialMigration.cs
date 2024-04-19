namespace Graphitty.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Filter",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        GroupID = c.Int(nullable: false),
                        Args = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.ID, t.GroupID, t.Args });
            
            CreateTable(
                "dbo.Graph",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AverageVertexDegree = c.Double(nullable: false),
                        BFSCode = c.String(nullable: false, unicode: false),
                        BFSCodeBitvector = c.String(unicode: false),
                        IsTCCFulfilled = c.Boolean(nullable: false),
                        LargestCliqueSize = c.Int(nullable: false),
                        MaxVertexDegree = c.Int(nullable: false),
                        MinimalColouringComplexityInMiliSeconds = c.Int(nullable: false),
                        NumCliquesOfSizeK = c.String(unicode: false),
                        NumDenserGraphsBFS = c.Int(nullable: false),
                        NumDenserGraphsProfile = c.Int(nullable: false),
                        NumEdges = c.Int(nullable: false),
                        NumGraphsWithSmallerEqualChromaticNumber = c.Int(nullable: false),
                        NumVertices = c.Int(nullable: false),
                        Profile = c.String(unicode: false),
                        TotalChromaticNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Graph");
            DropTable("dbo.Filter");
        }
    }
}
