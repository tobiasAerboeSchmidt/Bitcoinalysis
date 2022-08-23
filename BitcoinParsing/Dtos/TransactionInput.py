class TransactionInput:

    def __init__(self, outputTnx, outputIndex):
        self.outputTnx = outputTnx
        self.outputIndex = outputIndex

    def reprJSON(self):
        return dict(outputTnx=self.outputTnx, outputIndex=self.outputIndex)