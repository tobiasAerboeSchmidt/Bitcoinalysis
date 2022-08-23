class TransactionOutput:

    def __init__(self, address, index, value):
        self.address = address
        self.value = value
        self.index = index


    def reprJSON(self):
        return dict(address=self.address, value=self.value, index=self.index)