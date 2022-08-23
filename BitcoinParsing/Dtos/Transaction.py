class Transaction: 

    def __init__(self, hash, inputs, outputs):
        self.hash = hash
        self.inputs = inputs
        self.outputs = outputs

    def reprJSON(self):
        return dict(hash=self.hash, inputs=self.inputs, outputs=self.outputs)