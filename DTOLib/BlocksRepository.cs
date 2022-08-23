using DTOLib.DTOs;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTOLib
{
    public class BlocksRepository : RepositoryBase, IBlocksRepository
    {
        public BlocksRepository(AppDbContext context) : base(context)
        {
        }

        public async Task CreateBlock(List<BlockCreateDTO> dtos)
        {
            foreach (var dto in dtos)
            {
                foreach (var transaction in dto.Transactions)
                {
                    _context.Transactions.Add(new Transaction { TnxHash = transaction.Hash, BlockHash = dto.Hash });
                    foreach (var ti in transaction.Inputs)
                    {
                        _context.TransactionInputs.Add(new TransactionInput
                        {
                            OutputIndex = ti.OutputIndex,
                            OutputTnx = ti.OutputTnx,
                            TnxHash = transaction.Hash,
                        }
                        );
                    }

                    foreach (var to in transaction.Outputs)
                    {
                        _context.TransactionOutputs.Add(new TransactionOutput
                        {
                            Index = to.Index,
                            OutputWallet = to.Address,
                            TnxHash = transaction.Hash,
                            Value = to.Value
                        });
                    }
                }

                _context.Blocks.Add(new Block
                {
                    Hash = dto.Hash,
                    Date = dto.Date,
                });
            }

            await _context.SaveChangesAsync();
        }

        private Wallet CreateWalletIfNotExists(string address)
        {
            var wallet = _context.Wallets.FirstOrDefault(w => w.Address == address);
            if (wallet is null)
            {
                wallet = new Wallet { Address = address };
                _context.Wallets.Add(wallet);
                _context.SaveChanges();
            }
            return wallet;
        }


    }
}