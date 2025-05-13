import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '../base.component';
import { AccountInfoDetails, BudgetCategoryInfoDetails, FundInfoDetails } from '../data/accountInfoDetails';
import { Synagogue } from '../data/synagogue';
import { Toast } from '../toasts/toast';
import { ToastService } from '../toasts/toast.service';
import { ChartOfAccounts } from './chart-of-accounts';
import { ChartOfAccountsService } from './chart-of-accounts.service';

@Component({
  selector: 'app-chart-of-accounts',
  templateUrl: './chart-of-accounts.component.html',
  styleUrls: ['./chart-of-accounts.component.css'],
  providers: [
      ChartOfAccountsService
  ],
})
export class ChartOfAccountsComponent implements OnInit {

    private alive: boolean = true;
    name = 'Chart of Accounts';
    errorMessage: string = "";
    displayWait: boolean = false;
    synagogue: Synagogue;
    chartOfAccounts: ChartOfAccounts[] = [];
    accountType: string;
    includedDonorRpt: number;
    donationsAllowed: number;
    taxDeductibility: string;
    includedFundRaisingRpt: number;
    isActive: number;
    numberOfRecords: number;
    columns = [];
    bankAccounts: AccountInfoDetails[] = [];
    listOfFunds: FundInfoDetails[] = [];
    budgetCategories: BudgetCategoryInfoDetails[] = [];
    newAccount = ChartOfAccounts;
    searchValue: string = "";
    pageOfItems: Array<any>;

    constructor(
        private _baseComponent: BaseComponent,
        private _chartOfAccountsService: ChartOfAccountsService,
        private toastService: ToastService,
        private router: Router,
    ) {
    }

    ngOnInit() {
        this.getFunds();
        this.getBankAccounts();
        this.getBudgetCategories();
        this.setFilterDefaults();
        this.getChartOfAccounts();
    }

    onAccountCompleted() {
        this.getFunds();
        this.getBankAccounts();
        this.getBudgetCategories();
        this.getChartOfAccounts();
    }

    // get list of bank accounts
    getBankAccounts() {
        this.displayWait = true;
        this._chartOfAccountsService.getListOfAccounts().subscribe(
            response => {
                this.bankAccounts = response.filter(x => x.AccountNumber.startsWith("B"));
            },
            error => {
                this.errorMessage = <any>error;
                this._baseComponent.addErrorLog(error);
            },
            () => {
                this.displayWait = false;
            }
        );
    }

    // get list of funds
    getFunds() {
        this.displayWait = true;
        this._chartOfAccountsService.getListOfFunds().subscribe(
            response => {
                this.listOfFunds = response;
            },
            error => {
                this.errorMessage = <any>error;
                this._baseComponent.addErrorLog(error);
            },
            () => {
                this.displayWait = false;
            }
        );
    }

    // get list of budget categories
    getBudgetCategories() {
        this.displayWait = true;
        this._chartOfAccountsService.getBudgetCategories().subscribe(
            response => {
                this.budgetCategories = response;
            },
            error => {
                this.errorMessage = <any>error;
                this._baseComponent.addErrorLog(error);
            },
            () => {
                this.displayWait = false;
            }
        );
    }

    // set default values for filters
    setFilterDefaults() {
        this.accountType = "Select";
        this.includedDonorRpt = 2;
        this.donationsAllowed = 2;
        this.taxDeductibility = "Select";
        this.includedFundRaisingRpt = 2;
        this.isActive = 2;
        this.searchValue = "";
    }

    //  get list of chard of accounts
    getChartOfAccounts() {
        let accountObject = this;
        this.displayWait = true;
        this._chartOfAccountsService.getChartOfAccounts(this.accountType, this.includedDonorRpt, this.donationsAllowed, this.taxDeductibility, this.includedFundRaisingRpt, this.isActive, this.searchValue)
            .takeWhile(() => this.alive)
            .subscribe(
                response => {
                    accountObject.chartOfAccounts = response;
                    if (response != null) {
                        accountObject.numberOfRecords = accountObject.chartOfAccounts.length;
                    }
                },
                error => {
                    accountObject.errorMessage = <any>error;
                    accountObject._baseComponent.addErrorLog(error);
                },
                () => {
                    accountObject.displayWait = false;
                }
            );
    }

    //method to display notifications
    public displaySystemNotification(messageParm: string, messageType: number) {
        let toast = new Toast;
        toast.msg = messageParm;
        toast.msgType = messageType;
        toast.keepAfterNavigationChange = true;
        this.toastService.showToast(toast);
    }

    clearFilters() {
        this.setFilterDefaults();
        this.getChartOfAccounts();
    }

    returnToPreviousPage() {
        this.router.navigate(['/setupMenu']);
    }

    onChangePage(pageOfItems: Array<any>) {
        // update current page of items
        this.pageOfItems = pageOfItems;
    }

    getIndex(dictionaryAccountGUID: string) {
        return this.chartOfAccounts.findIndex(x => x.DictionaryAccountGUID == dictionaryAccountGUID);
    }

    setMaxLength(event, length) {
        if (event.target.value.length >= length)
            return false;
        return true;
    }
}
