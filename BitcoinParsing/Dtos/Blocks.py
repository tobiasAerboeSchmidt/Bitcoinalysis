class Blocks: 

    def __init__(self, blocks):
        self.blocks = blocks

    def reprJSON(self):
        return dict(blocks=self.blocks)


