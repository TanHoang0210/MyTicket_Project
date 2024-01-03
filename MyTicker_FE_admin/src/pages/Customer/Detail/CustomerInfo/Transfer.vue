<template>
    <div class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">
                    <card class="strpied-tabled-with-hover" body-classes="table-full-width table-responsive">
                        <template slot="header">
                            <div class="row" style="display: flex; justify-content: space-between;">
                                <div class="col-md-6">
                                    <h4 class="card-title">Danh sách chuyển nhượng vé khách hàng</h4>
                                </div>
                                <div class="col-md-4">
                                    <b-input-group class="float-right">
                                        <b-form-input v-model="search.keyword"></b-form-input>
                                        <b-input-group-append>
                                            <b-button class="btn-search" @click="getAllData()" variant="outline-success">Tìm
                                                kiếm</b-button>
                                        </b-input-group-append>
                                    </b-input-group>
                                </div>
                            </div>
                        </template>
                        <div class="row">
                            <div class="col-md-10">

                            </div>
                            <div class="col-md-2">
                                <label for="per-page-select" style="text-align: end;">Số bản ghi</label>
                                <b-form-select style="width:50% right:0" id="per-page-select" v-model="pageSize"
                                    @change="getAllData()" :options="pageSizeOption" size="sm"></b-form-select>
                            </div>
                        </div>
                        <b-table class="custom-table" :current-page="pageNumber" id="table-transfer" striped hover :fields="fields"
                            :items="orders">
                            <template #cell(transferStatus)="data">
                                <td style="color: blue;font-weight: 600;" v-if="data.item.transferStatus === 1">Khởi tạo
                                </td>
                                <td style="color: green;font-weight: 600;" v-if="data.item.transferStatus === 2">Đã xác nhận
                                </td>
                                <td style="color: #888;font-weight: 600;" v-if="data.item.transferStatus === 3">Đã hủy</td>
                                <td style="color: yellow;font-weight: 600;" v-if="data.item.transferStatus === 4">Đang thanh
                                    toán</td>
                                <td style="color: orange;font-weight: 600;" v-if="data.item.transferStatus === 5">Đã chuyển
                                    nhượng</td>
                            </template>
                            <template #cell(action)="data">
                                <div style="font-size: 1.2rem; !important">
                                    <b-button @click="showModalTransfer(data.item.id)" class="table-btn" variant="secondary"
                                        title="Xem chi tiết">
                                        <b-icon icon="pencil-square">
                                        </b-icon>
                                    </b-button>
                                    <b-button v-if="data.item.transferRefundRequest" @click="Refund(data.item.id,customerId)" class="table-btn"
                                        variant="warning" title="Hoàn tiền">
                                        <b-icon icon="currency-exchange">
                                        </b-icon>
                                    </b-button>
                                </div>
                            </template>
                        </b-table>
                        <b-pagination v-model="pageNumber" :total-rows="totals" :per-page="pageSize"
                            aria-controls="table-transfer"></b-pagination>
                    </card>
                    <b-modal id="modal-add-edit" scrollable ref="modal-add-edit" size="lg" title="Thông tin chuyển nhượng"
                        ok-only>
                        <form>
                            <div class="row">
                                <div class="col-md-12">
                                    <base-input type="text" label="Tên Sự kiện" :disabled="true"
                                        v-model="transfer.eventName">
                                    </base-input>
                                </div>
                                <div class="col-md-6">
                                    <base-input type="text" label="Mã đặt vé" v-model="transfer.orderCode">
                                    </base-input>
                                    <base-input type="text" label="Ngày đặt vé" v-model="transfer.orderDate">
                                    </base-input>
                                    <base-input type="text" label="Ngày diễn ra" v-model="transfer.organizationDay">
                                    </base-input>
                                    <base-input type="text" label="Sân vận động" v-model="transfer.venueName">
                                    </base-input>
                                    <base-input type="text" label="Địa chỉ" v-model="transfer.venueAddress">
                                    </base-input>
                                    <base-input type="text" label="Ngày yêu cầu chuyển nhượng"
                                        v-model="transfer.transferDate">
                                    </base-input>
                                </div>
                                <div class="col-md-6">
                                    <base-input type="text" label="Hạng vé" v-model="transfer.ticketEventName">
                                    </base-input>
                                    <base-input type="text" label="Mã vé" v-model="transfer.ticketCode">
                                    </base-input>
                                    <base-input type="text" label="Mã chỗ ngồi" v-model="transfer.seatCode">
                                    </base-input>
                                    <base-input type="text" label="Giá" v-model="transfer.price">
                                    </base-input>
                                    <base-input v-if="transfer.transferStatus === 3" type="text"
                                        label="Ngày hủy chuyển nhượng" v-model="transfer.transferCancelDate">
                                    </base-input>
                                    <base-input v-if="transfer.transferStatus === 5" type="text" label="Ngày chuyển nhượng"
                                        v-model="transfer.transferDoneDate">
                                    </base-input>
                                    <label for="status">Trạng thái</label>
                                    <br>
                                    <span style="color: blue;font-weight: 600;" v-if="transfer.transferStatus === 1">Khởi
                                        tạo
                                    </span>
                                    <span style="color: green;font-weight: 600;" v-if="transfer.transferStatus === 2">Đã xác
                                        nhận
                                    </span>
                                    <span style="color: #888;font-weight: 600;" v-if="transfer.transferStatus === 3">Đã
                                        hủy</span>
                                    <span style="color: yellow;font-weight: 600;" v-if="transfer.transferStatus === 4">Đang
                                        thanh
                                        toán</span>
                                    <span style="color: orange;font-weight: 600;" v-if="transfer.transferStatus === 5">Đã
                                        chuyển
                                        nhượng</span>
                                </div>
                            </div>
                        </form>
                        <template #modal-footer="{ ok, cancel }">
                            <div style="margin: auto; width: 100%;">
                                <b-button class="buttonModal" size="lg" variant="secondary" @click="cancel()">
                                    Đóng
                                </b-button>
                            </div>
                        </template>
                    </b-modal>
                </div>
            </div>
        </div>
    </div>
</template>
    
<script>
import LTable from 'src/components/Table.vue'
import Card from 'src/components/Cards/Card.vue'
import axios from 'axios'
import helpService from 'src/service/help/helpService'
export default {
    components: {
        LTable,
        Card
    },
    data() {
        return {
            search: {
                keyword: null,
                status: null
            },
            totals: 0,
            pageSizeOption: [5, 10, 25, 50, 100],
            id: 0,
            isUpdate: false,
            pageSize: 100,
            pageNumber: 1,
            orders: [],
            imageUpload: null,
            fields: ['id',
                { key: 'orderCode', label: 'Mã đơn đặt vé ' },
                { key: 'orderDate', label: 'Ngày đặt vé' },
                { key: 'eventName', label: 'Tên sự kiện' ,class: 'max-width-column'},
                { key: 'ticketEventName', label: 'Hạng vé' },
                { key: 'transferStatus', label: 'Trạng thái' },
                { key: 'action', label: 'Thao tác' },
            ],
            transfer: {
                id: 0,
                orderId: 0,
                orderCode: "string",
                orderDate: "2023-12-24T15:35:00.181Z",
                eventDetailId: 0,
                eventName: "string",
                organizationDay: "2023-12-24T15:35:00.181Z",
                venueName: "string",
                venueAddress: "string",
                ticketId: 0,
                ticketEventName: "string",
                ticketCode: "string",
                seatCode: "string",
                price: 0,
                transferStatus: 0,
                transferDate: "2023-12-24T15:35:00.181Z",
                transferDoneDate: "2023-12-24T15:35:00.181Z",
                transferCancelDate: "2023-12-24T15:35:00.181Z"
            }
        };
    },
    methods: {
        async getOrder() {
            try {
                const res = await axios.get(
                    "myticket/api/order/admin/transfer/find-all",
                    {
                        params: {
                            customerId: this.$route.query.id,
                            pageSize: this.pageSize,
                            pageNumber: this.pageNumber,
                            keyword: this.search.keyword,
                            status: this.search.status
                        },
                    }
                )
                this.totals = res.data.data.totalItems
                return res.data.data.items;
            } catch (error) {
                console.error('API Error:', error);
                throw error;
            }
        },
        handleFileChange(event) {
            // Update the 'file' data property when the file input changes
            this.imageUpload = event.target.files[0];
        },
        async uploadImageType() {
            const formData = new FormData();
            formData.append('file', this.imageUpload);
            await axios.post('myticket/api/file/upload', formData).then(
                res => this.updateSupplier.image = res.data.data
            ).catch(
                err => err
            )
            console.log(this.updateSupplier.image)
        },
        async getAllData() {
            try {
                this.orders = await this.getOrder();
                this.orders.forEach(element => {
                    element.orderDate = helpService.formatDate(element.orderDate)
                });
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        },
        async getTransferById(id) {
            try {
                const res = await axios.get(
                    "myticket/api/order/transfer/find-by-id",
                    {
                        params: {
                            id: id,
                        },
                    }
                )
                return res.data.data;
            } catch (error) {
                console.error('API Error:', error);
                throw error;
            }
        },
        notifyVue(type, message, verticalAlign, horizontalAlign, color) {
            this.$notifications.notify(
                {
                    message: `<span><b>${type}</b> - ${message}</span>`,
                    icon: 'nc-icon nc-app',
                    horizontalAlign: horizontalAlign,
                    verticalAlign: verticalAlign,
                    type: color
                })
        },
        async showModalTransfer(id) {
            console.log(id)
            try {
                this.transfer = await this.getTransferById(id)
                this.transfer.transferCancelDate = helpService.formatDate(this.transfer.transferCancelDate)
                this.transfer.transferDate = helpService.formatDate(this.transfer.transferDate)
                this.transfer.transferDoneDate = helpService.formatDate(this.transfer.transferDoneDate)
                this.transfer.orderDate = helpService.formatDate(this.transfer.orderDate)
                this.transfer.organizationDay = helpService.formatDate(this.transfer.organizationDay)
                this.transfer.price = helpService.formatCurrency(this.transfer.price)
                // this.venues = await this.findAllVenue();
                this.$nextTick(() => {
                    // Using $nextTick to ensure the modal component is updated
                    this.$refs['modal-add-edit'].show();
                });
            } catch (error) {
                console.error("Error fetching ticket details:", error);
            }
        },
        async addEditSupplier() {
            if (this.updateSupplier.id !== 0) {
                const response = await axios.put('myticket/api/user/update-by-suplier', {
                    fullName: this.updateSupplier.fullName,
                    shortName: this.updateSupplier.shortName,
                    address: this.updateSupplier.address,
                    taxCode: this.updateSupplier.taxCode,
                    id: this.updateSupplier.id
                }, {
                    headers: {
                        'Content-Type': 'application/json'
                    },
                })
                if (response.data.code === 200) {
                    this.notifyVue('Thành công', 'Cập nhật nhà cung cấp thành công', 'top', 'right', 'success')
                }
                else {
                    this.notifyVue('Thất bại', 'Cập nhật nhà cung cấp thất bại', 'top', 'right', 'danger')
                }
            } else {
                const response = await axios.post('myticket/api/user/add-by-suplier', {
                    fullName: this.updateSupplier.fullName,
                    shortName: this.updateSupplier.shortName,
                    address: this.updateSupplier.address,
                    taxCode: this.updateSupplier.taxCode,
                }, {
                    headers: {
                        'Content-Type': 'application/json'
                    },
                })
                if (response.data.code === 200) {
                    this.notifyVue('Thành công', 'Thêm nhà cung cấp thành công', 'top', 'right', 'success')
                }
                else {
                    this.notifyVue('Thất bại', 'Thêm nhà cung cấp thất bại', 'top', 'right', 'danger')
                }
            }
            this.$refs['modal-add-edit'].hide();
            this.getAllData();
        },
        async addEditSupplierAccount(supplierId) {
            if (this.updateAccount.id !== 0) {
                const response = await axios.put('myticket/api/user/account/update-by-suplier', {
                    username: this.updateAccount.username,
                    password: this.updateAccount.password,
                    email: this.updateAccount.email,
                    phone: this.updateAccount.phone,
                    id: this.updateAccount.id
                }, {
                    headers: {
                        'Content-Type': 'application/json'
                    },
                })
                if (response.data.code === 200) {
                    this.notifyVue('Thành công', 'Cập nhật tài khoản nhà cung cấp thành công', 'top', 'right', 'success')
                }
                else {
                    this.notifyVue('Thất bại', 'Cập nhật tài khoản nhà cung cấp thất bại', 'top', 'right', 'danger')
                }
            } else {
                const response = await axios.post('myticket/api/user/account/add-by-suplier', {
                    supplierId: supplierId,
                    username: this.updateAccount.username,
                    password: this.updateAccount.password,
                    phone: this.updateAccount.phone,
                    email: this.updateAccount.email,
                }, {
                    headers: {
                        'Content-Type': 'application/json'
                    },
                })
                if (response.data.code === 200) {
                    this.notifyVue('Thành công', 'Thêm tài khoản nhà cung cấp thành công', 'top', 'right', 'success')
                }
                else {
                    this.notifyVue('Thất bại', 'Thêm tài khoản nhà cung cấp thất bại', 'top', 'right', 'danger')
                }
            }
            this.$refs['modal-add-edit-account'].hide();
            this.$refs['modal-add-edit'].show();
        },
    },
    mounted() {
        this.getAllData();
    },
    computed: {
    },
    watch: {
        // Fetch data when the pageNumber changes
        pageNumber(newPage, oldPage) {
            if (newPage !== oldPage) {
                this.getAllData();
            }
        },
    },
};
</script>
    
<style>
.table-btn.btn {
    border: none !important;
}
.custom-table .max-width-column {
  /* Set your desired max width */
  max-width: 200px;
  overflow-x: auto;
  white-space: nowrap;
}
</style>
    