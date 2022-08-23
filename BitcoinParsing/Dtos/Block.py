class Block : 

    def __init__(self, hash, date, transactions):
        self.hash = hash
        self.date = date
        self.transactions = transactions

    def reprJSON(self):
        return dict(hash=self.hash, date=self.date, transactions=self.transactions)


