import os
import json
import requests
from blockchain_parser.blockchain import Blockchain
from Dtos.Blocks import *
from Dtos.Block import *
from Dtos.Transaction import *
from Dtos.TransactionInput import *
from Dtos.TransactionOutput import *
from Dtos.ComplexEncoder import *
from urllib3.exceptions import InsecureRequestWarning

requests.packages.urllib3.disable_warnings(category=InsecureRequestWarning)

def send_post_request(block_json):
    # Post 100 blocks
    response = requests.post(url = "http://localhost:5128/Blocks", json=json.loads(block_json), verify=False)  
    status_code = response.status_code
    if status_code != 200:
        print(response.status_code)
        print(block_json)

# Instantiate the Blockchain by giving the path to the directory
# containing the .blk files created by bitcoind
blockchain = Blockchain('C:/Users/tschmidt/AppData/Roaming/Bitcoin/blocks')
for block in blockchain.get_unordered_blocks():
    transactions = []
    for tx in block.transactions:
        
        transaction_inputs = []
        transaction_outputs = []

        # Inputs
        for input in tx.inputs:
            # Skip coinbase inputs
            if input.transaction_index != 4294967295:
                transaction_inputs.append(TransactionInput(input.transaction_hash, input.transaction_index))

        # Outputs
        for no, output in enumerate(tx.outputs):
            if len(output.addresses) > 0:
                transaction_outputs.append(TransactionOutput(output.addresses[0].address, no, output.value))

        transaction = Transaction(tx.txid, transaction_inputs, transaction_outputs)
        transactions.append(transaction)

    timestamp = str(block.header.timestamp).replace(" ", "T") + "Z"
    block_dto = Blocks([Block(block.hash, timestamp, transactions)])
    block_json = json.dumps(block_dto.__dict__, cls=ComplexEncoder)
    send_post_request(block_json)

