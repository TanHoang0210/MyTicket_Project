<template>
    <div class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <card>
                        <div class="row">
                            <h4 slot="header" class="card-title col-md-6">Thông tin chi tiết sự kiện</h4>
                            <div class="text-center col-md-6">
                                <button v-if="eventInfo.status === 5" type="button" style="margin: 0 5px;"
                                    class="btn btn-success btn-fill float-right "
                                    @click.prevent="updateEventStatus(eventInfo.id, 1)">
                                    Mở Sự kiện
                                </button>
                                <button v-else-if="eventInfo.status === 1 || eventInfo.status === 3" type="button"
                                    style="margin: 0 5px;" class="btn btn-warning btn-fill float-right "
                                    @click.prevent="updateEventStatus(eventInfo.id, 5)">
                                    Hủy Sự kiện
                                </button>
                                <button v-if="!isUpdate" style="margin: 0 5px;" type="button"
                                    class="btn btn-info btn-fill float-right " @click="setUpdate">
                                    Cập nhật
                                </button>
                                <div v-else>
                                    <button type="button" style="margin: 0 5px;"
                                        class="btn btn-secondary btn-fill float-right " @click="cancelUpdate">
                                        Hủy
                                    </button>
                                    <button type="submit" style="margin: 0 5px;" class="btn btn-info btn-fill float-right "
                                        @click.prevent="updateProfile">
                                        Lưu lại
                                    </button>
                                </div>
                            </div>
                        </div>
                        <form>
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Tên sự kiện</label>
                                    <textarea type="text" label="Tên sự kiện" class="form-control border-input"
                                        :disabled="!isUpdate" placeholder="Light dashboard" v-model="eventInfo.eventName">
                                    </textarea>
                                </div>
                                <div class="col-md-3">
                                    <base-input type="text" label="Nhà cung cấp" :disabled="true"
                                        placeholder="Light dashboard" v-model="eventInfo.supllier">
                                    </base-input>
                                </div>
                                <div class="col-md-3">
                                    <label for="eventTypeId">Loại Sự Kiện</label>
                                    <br>
                                    <select :disabled="!isUpdate" class="select-form" v-model="eventInfo.eventTypeId"
                                        label="eventTypeId" name="eventTypeId" id="eventTypeId">
                                        <option v-for="item in listType" :value="item.id">{{ item.name }}</option>
                                    </select>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Mô tả sự kiện</label>
                                        <textarea :disabled="!isUpdate" rows="10" class="form-control border-input"
                                            placeholder="Here can be your description" v-model="eventInfo.eventDescription">
                                    </textarea>
                                    </div>
                                    <b-form-checkbox :disabled="!isUpdate" id="checkbox-1" v-model="eventInfo.isExchange"
                                        name="checkbox-1">
                                        Có cho trả vé không?
                                    </b-form-checkbox>
                                </div>
                                <div class="col-md-8">
                                    <label for="image">Ảnh sự kiện</label>
                                    <br>
                                    <img style="max-width: 100%;" slot="image" :src="$fileUrl + eventInfo.eventImage"
                                        alt="..." />
                                    <br>
                                    <div style="display: flex; margin: 10px;">
                                        <div style="margin: auto;">
                                            <input v-if="isUpdate" @change="handleFileChange" type="file" id="myFile"
                                                name="filename">
                                            <button v-if="isUpdate" @click="uploadEventImage"
                                                class="btn btn-info btn-fill btn-img">
                                                <i class="nc-icon nc-cloud-upload-94"></i>
                                            </button>
                                            <button class="btn btn-info btn-fill btn-img">
                                                <i class="nc-icon nc-zoom-split"></i>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label>Đuy định đổi trả</label>
                                        <textarea :disabled="!isUpdate" rows="5" class="form-control border-input"
                                            placeholder="Here can be your description" v-model="eventInfo.exchangePolicy">
                                        </textarea>
                                    </div>
                                    <div class="form-group">
                                        <label>Nội quy sự kiện</label>
                                        <textarea :disabled="!isUpdate" rows="5" class="form-control border-input"
                                            placeholder="Here can be your description" v-model="eventInfo.admissionPolicy">
                                        </textarea>
                                    </div>
                                </div>
                            </div>
                        </form>
                    </card>
                    <card>
                        <div v-if="eventInfo.eventDetails.length > 0">
                            <h4 slot="header" class="card-title">Danh sách các ngày diễn ra sự kiện
                            </h4>
                            <b-table striped hover :fields="detailEventField" :items="eventInfo.eventDetails">
                                <template #row-details="data">
                                    <b-card>
                                        <b-table striped hover :fields="ticketField" :items="data.item.ticketEvents">
                                            <template #cell(status)="ticket">
                                                <span v-if="ticket.item.status === 1">Mở bán vé</span>
                                                <span v-if="ticket.item.status === 2">Ngừng bán vé</span>
                                            </template>
                                            <template #cell(action)="ticket">
                                                <div style="font-size: 1.2rem; !important">
                                                    <b-button class="table-btn" @click="deleteTicket()" variant="danger"
                                                        title="Xóa loại vé">
                                                        <b-icon icon="trash">
                                                        </b-icon>
                                                    </b-button>
                                                    <b-button @click.prevent="showModalTicket(ticket.item.id, data.item.id)"
                                                        title="Chi tiết" class="btn btn-info" :style="{ border: 'none' }">
                                                        <b-icon icon="pencil-square">
                                                        </b-icon>
                                                    </b-button>
                                                    <b-button @click.prevent="showModalTicketList(ticket.item.id)"
                                                        title="Danh sách vé" class="btn btn-info" :style="{ border: 'none' }">
                                                        <b-icon icon="list">
                                                        </b-icon>
                                                    </b-button>
                                                    <b-button @click="updateStatusTicket(ticket.item.id, 2)"
                                                        v-if="ticket.item.status === 1" class="btn btn-warning"
                                                        :style="{ border: 'none' }" title="Ngừng bán vé">
                                                        <b-icon icon="lock"></b-icon>
                                                    </b-button>
                                                    <b-button @click="updateStatusTicket(ticket.item.id, 1)"
                                                        v-if="ticket.item.status === 2" class="btn btn-success"
                                                        :style="{ border: 'none' }" title="Mở bán vé">
                                                        <b-icon icon="unlock"></b-icon>
                                                    </b-button>
                                                </div>
                                            </template>
                                        </b-table>
                                        <b-button type="button" style="margin: 0 5px;"
                                            class="btn btn-warning btn-fill float-right "
                                            @click.prevent="showModalTicket(0, data.item.id)">
                                            Thêm mới loại vé
                                        </b-button>
                                    </b-card>
                                </template>
                                <template #cell(status)="data">
                                    <td style="color: blue;font-weight: 600;" v-if="data.item.status === 1">Khởi tạo</td>
                                    <td style="color: greenyellow;font-weight: 600;" v-if="data.item.status === 2">Mở bán
                                        vé</td>
                                    <td style="color: orange;font-weight: 600;" v-if="data.item.status === 3">Đóng bán vé
                                    </td>
                                    <td style="color: #888;font-weight: 600;" v-if="data.item.status === 4">Đang diễn ra
                                    </td>
                                    <td style="color: red;font-weight: 600;" v-if="data.item.status === 5">Đã hủy</td>
                                </template>
                                <template #cell(action)="data">
                                    <div style="font-size: 1.2rem; !important">
                                        <b-button @click="showModalUpdateDetail(data.item)" class="table-btn"
                                            variant="secondary" title="Xem chi tiết">
                                            <b-icon icon="pencil-square">
                                            </b-icon>
                                        </b-button>
                                        <b-button v-if="data.item.status === 3 || data.item.status === 1" title="Mở bán vé"
                                            type="button" style=" border: none;" variant="success" class="table-btn "
                                            @click.prevent="updateEventDetailStatus(data.item.id, 2)">
                                            <b-icon icon="unlock"></b-icon>
                                        </b-button>
                                        <b-button v-if="data.item.status === 2" type="button" style="border: none;"
                                            class="table-btn" variant="warning" title="Đóng bán vé"
                                            @click.prevent="updateEventDetailStatus(data.item.id, 3)">
                                            <b-icon icon="lock"></b-icon>
                                        </b-button>
                                        <b-button v-if="data.item.status === 1 || data.item.status === 3"
                                            title="Hủy sự kiện" type="button" style="border: none;" class="table-btn "
                                            variant="danger" @click.prevent="updateEventDetailStatus(data.item.id, 5)">
                                            <b-icon icon="toggle2-off"></b-icon>
                                        </b-button>
                                        <b-button v-if="data.item.status === 5" title="Mở sự kiện" type="button"
                                            style="border: none;" class="table-btn " variant="success"
                                            @click.prevent="updateEventDetailStatus(data.item.id, 1)">
                                            <b-icon icon="toggle2-on"></b-icon>
                                        </b-button>
                                        <b-button type="button" style="border: none;" class="table-btn"
                                            @click="data.toggleDetails()">
                                            <b-icon icon="eye" title="Xem danh sách vé"
                                                v-if="!data.detailsShowing"></b-icon>
                                            <b-icon icon="eye-slash" title="Ẩn danh sách vé" v-else></b-icon>
                                        </b-button>
                                    </div>
                                </template>
                            </b-table>
                        </div>
                    </card>
                    <div style="display: flex;">
                        <button style="margin: auto;" type="button" class="btn btn-info btn-fill"
                            @click="showModalAddDetail">
                            Thêm sự kiện
                        </button>
                    </div>
                </div>
            </div>
            <b-modal id="modal-add-detail" ref="modal-add-detail" size="xl" title="Thêm chi tiết sự kiện">
                <form>
                    <div class="row">
                        <div class="col-md-3">
                            <label for="eventTypeId">Sân vận động</label>
                            <br>
                            <select class="select-form" v-model="addEventDetail.venueId" label="eventTypeId"
                                name="eventTypeId" id="eventTypeId">
                                <option v-for="item in venues" :value="item.id">{{ item.name }}</option>
                            </select>
                        </div>
                        <div class="col-md-3">
                            <label for="example-datepicker">Ngày diễn ra</label>
                            <input v-model="addEventDetail.organizationDay" type="datetime-local" id="datetime"
                                name="datetime">
                        </div>
                        <div class="col-md-3">
                            <label for="example-datepicker">Ngày Mở bán vé</label>
                            <input v-model="addEventDetail.startSaleTicketDate" type="datetime-local" id="datetime"
                                name="datetime">
                        </div>
                        <div class="col-md-3">
                            <label for="example-datepicker">Ngày đóng bán vé</label>
                            <input v-model="addEventDetail.endSaleTicketDate" type="datetime-local" id="datetime"
                                name="datetime">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <b-form-checkbox id="checkbox-1" v-model="addEventDetail.havingSeatMap" name="checkbox-1">
                                Có Sơ đồ chọn chỗ
                            </b-form-checkbox>
                            <label for="example-datepicker">Kiểu chọn vé</label>
                            <select class="select-form" v-model="addEventDetail.selectSeatType" label="selectSeatType"
                                name="selectSeatType" id="selectSeatType">
                                <option v-for="item in selectSeatTypes" :value="item.type">{{ item.name }}</option>
                            </select>
                        </div>
                        <div class="col-md-9">
                            <label for="image">Ảnh sự kiện</label>
                            <br>
                            <img v-if="addEventDetail.eventSeatMapImage" style="max-width: 780px;" slot="image"
                                :src="$fileUrl + addEventDetail.eventSeatMapImage" alt="..." />
                            <br>
                            <div style="display: flex; margin: 10px;">
                                <div style="margin: auto;">
                                    <form>
                                        <label v-if="addEventDetail.eventSeatMapImage === null" for="">Tải lên ảnh sự
                                            kiện</label>
                                        <br>
                                        <input @change="handleFileChange" type="file" id="myFile" name="filename">
                                        <button type="button" @click="uploadImage()" class="btn btn-info btn-fill btn-img">
                                            <i class="nc-icon nc-cloud-upload-94"></i>
                                        </button>
                                        <button type="button" class="btn btn-info btn-fill btn-img">
                                            <i class="nc-icon nc-zoom-split"></i>
                                        </button>
                                    </form>

                                </div>
                            </div>
                        </div>
                    </div>
                </form>
                <template #modal-footer="{ ok, cancel }">
                    <div style="margin: auto; width: 30%;">
                        <b-button class="buttonModal" size="lg" variant="success" @click="addEventDetailFunc()">
                            OK
                        </b-button>
                        <b-button class="buttonModal" size="lg" variant="secondary" @click="cancel()">
                            Cancel
                        </b-button>
                    </div>
                </template>
            </b-modal>
            <b-modal id="modal-update-detail" ref="modal-update-detail" size="xl" title="Thêm chi tiết sự kiện">
                <form>
                    <div class="row">
                        <div class="col-md-3">
                            <label for="eventTypeId">Sân vận động</label>
                            <br>
                            <select class="select-form" v-model="updateEventDetail.venueId" label="eventTypeId"
                                name="eventTypeId" id="eventTypeId">
                                <option v-for="item in venues" :value="item.id">{{ item.name }}</option>
                            </select>
                            <label for="example-datepicker">Ngày diễn ra:</label>
                            <flat-pickr v-model="updateEventDetail.organizationDay"
                                :config="dateTimePickerConfig"></flat-pickr>
                            <label for="example-datepicker">Ngày Mở bán vé</label>
                            <flat-pickr v-model="updateEventDetail.startSaleTicketDate"
                                :config="dateTimePickerConfig"></flat-pickr>
                            <label for="example-datepicker">Ngày đóng bán vé</label>
                            <flat-pickr v-model="updateEventDetail.endSaleTicketDate"
                                :config="dateTimePickerConfig"></flat-pickr>
                            <b-form-checkbox id="checkbox-1" v-model="updateEventDetail.havingSeatMap" name="checkbox-1">
                                Có Sơ đồ chọn chỗ
                            </b-form-checkbox>
                            <label for="example-datepicker">Kiểu chọn vé</label>
                            <select class="select-form" v-model="updateEventDetail.selectSeatType" label="selectSeatType"
                                name="selectSeatType" id="selectSeatType">
                                <option v-for="item in selectSeatTypes" :value="item.type">{{ item.name }}</option>
                            </select>
                        </div>
                        <div class="col-md-9">
                            <label for="image">Ảnh sự kiện</label>
                            <br>
                            <img v-if="updateEventDetail.eventSeatMapImage" style="max-width: 780px;" slot="image"
                                :src="$fileUrl + updateEventDetail.eventSeatMapImage" alt="..." />
                            <br>
                            <div style="display: flex; margin: 10px;">
                                <div style="margin: auto;">
                                    <form>
                                        <label v-if="updateEventDetail.eventSeatMapImage === null" for="">Tải lên ảnh sự
                                            kiện</label>
                                        <br>
                                        <input @change="handleFileChange" type="file" id="myFile" name="filename">
                                        <button type="button" @click="uploadImageUpdateSeat()"
                                            class="btn btn-info btn-fill btn-img">
                                            <i class="nc-icon nc-cloud-upload-94"></i>
                                        </button>
                                        <button type="button" class="btn btn-info btn-fill btn-img">
                                            <i class="nc-icon nc-zoom-split"></i>
                                        </button>
                                    </form>

                                </div>
                            </div>
                        </div>
                    </div>
                </form>
                <template #modal-footer="{ ok, cancel }">
                    <div style="margin: auto; width: 30%;">
                        <b-button class="buttonModal" size="lg" variant="success" @click="updateEventDetailFunc()">
                            OK
                        </b-button>
                        <b-button class="buttonModal" size="lg" variant="secondary" @click="cancel()">
                            Cancel
                        </b-button>
                    </div>
                </template>
            </b-modal>
            <b-modal id="modal-add-edit-ticket" ref="modal-add-edit-ticket" size="lg" title="Thông tin loại vé">
                <form>
                    <div class="row">
                        <div class="col-md-12">
                            <base-input type="text" label="Hạng vé" placeholder="Nhập tên hạng vé"
                                v-model="updateTicket.name">
                            </base-input>
                            <base-input type="text" label="Giá" placeholder="Nhập giá vé" v-model="updateTicket.price">
                            </base-input>
                            <base-input type="number" label="Số lượng" placeholder="Nhập số lượng"
                                v-model="updateTicket.quantity">
                            </base-input>
                        </div>
                    </div>
                </form>
                <template #modal-footer="{ ok, cancel }">
                    <div style="margin: auto; width: 30%;">
                        <b-button class="buttonModal" size="lg" variant="success" @click="updateTicketFunc()">
                            OK
                        </b-button>
                        <b-button class="buttonModal" size="lg" variant="secondary" @click="cancel()">
                            Cancel
                        </b-button>
                    </div>
                </template>
            </b-modal>
            <b-modal id="modal-list-ticket" ref="modal-list-ticket" size="lg" title="Danh sách vé">
                    <b-table responsive striped hover :fields="ticketListField" :items="myListTicket">
                                    <template #cell(status)="data">
                                        <td style="color: green;font-weight: 600;" v-if="data.item.status === 1">Sẵn sàng</td>
                                        <td style="color: #555;font-weight: 600;" v-if="data.item.status === 2">Đã bán
                                        </td>
                                    </template>
                                </b-table>
                <template #modal-footer="{cancel }">
                    <div style="margin: auto; width: 30%;">
                        <b-button class="buttonModal" size="lg" variant="secondary" @click="cancel()">
                            Cancel
                        </b-button>
                    </div>
                </template>
            </b-modal>
        </div>
    </div>
</template>
<script>
import FlatPickr from 'vue-flatpickr-component';
import 'flatpickr/dist/flatpickr.css';
import Card from 'src/components/Cards/Card.vue'
import axios from 'axios'
import helpService from 'src/service/help/helpService'
import moment from 'moment'
export default {
    components: {
        Card, FlatPickr
    },
    data() {
        return {
            dateTimePickerConfig: {
                enableTime: true,
                dateFormat: 'H:i d/m/Y',
            },
            disabledIndices: [],
            isUpdate: false,
            myListTicket:[],
            listType: [
                {

                    id: 0,
                    name: "string",
                    description: "string",
                    eventTypeImage: "string"
                }
            ],
            _showDetails: true,
            selectSeatTypes: [
                {
                    type: 1,
                    name: "Chọn vé"
                },
                {
                    type: 2,
                    name: "Chọn hạng vé"
                }
            ],
            ticketField: ['id',
                { key: 'name', label: 'Hạng vé' },
                { key: 'price', label: 'Giá' },
                { key: 'status', label: 'Trạng thái' },
                { key: 'intQuantity', label: 'Số lượng' },
                { key: 'quantity', label: 'Số lượng còn lại' },
                { key: 'action', label: 'Thao tác' },
            ],
            ticketListField:[
                'id',
                { key: 'ticketCode', label: 'Mã vé' },
                { key: 'seatCode', label: 'Mã chỗ ngồi' },
                { key: 'status', label: 'Trạng thái' },
            ],
            detailEventField: [
                { key: 'id' },
                { key: 'organizationDay', label: 'Ngày diễn ra' },
                { key: 'startSaleTicketDate', label: 'Ngày mở bán vé' },
                { key: 'endSaleTicketDate', label: 'Ngày đóng bán vé' },
                { key: 'status', label: 'Trạng thái' },
                { key: 'action', label: 'Thao tác' },
                // { key: 'ticketEvents', label: 'Danh sách vé' },
            ],
            addEventDetail: {
                eventId: 0,
                venueId: 0,
                organizationDay: null,
                startSaleTicketDate: null,
                endSaleTicketDate: null,
                eventSeatMapImage: null,
                havingSeatMap: true,
                selectSeatType: 0
            },
            updateEventDetail: {
                id: 0,
                eventId: 0,
                venueId: 0,
                organizationDay: "2023-12-20T09:20:20.631Z",
                startSaleTicketDate: "2023-12-20T09:20:20.631Z",
                endSaleTicketDate: "2023-12-20T09:20:20.631Z",
                eventSeatMapImage: "string",
                havingSeatMap: true,
                selectSeatType: 0
            },
            updateEvent: {
                eventName: null,
                eventTypeId: 0,
                supplierId: 0,
                eventDescription: null,
                exchangePolicy: null,
                admissionPolicy: null,
                eventImage: null,
                isExchange: true,
                id: 0
            },
            seatImage: null,
            venues: [],
            eventDetailTable: [],
            listTicket: [
                {
                    id: 0,
                    name: "string",
                    price: 0,
                    eventDetailId: 0,
                    status: 0,
                    quantity: 0,
                    intQuantity: 0
                }
            ],
            eventInfo: {
                id: 0,
                eventName: "string",
                eventTypeId: 0,
                eventTypeName: "1",
                eventDescription: "string",
                eventImage: "string",
                admissionPolicy: "string",
                exchangePolicy: "string",
                status: 0,
                firstEventDate: "2023-12-10T12:36:01.468Z",
                lastEventDate: "2023-12-10T12:36:01.468Z",
                supllier: "string",
                eventDetails: [
                    {
                        id: 0,
                        eventId: 0,
                        eventName: "string",
                        eventImage: "string",
                        venueId: 0,
                        venueName: "string",
                        organizationDay: "2023-12-10T12:36:01.468Z",
                        startSaleTicketDate: "2023-12-10T12:36:01.468Z",
                        endSaleTicketDate: "2023-12-10T12:36:01.468Z",
                        eventSeatMapImage: "string",
                        seatSelectType: 0,
                        havingSeatMap: true,
                        status: 0,
                        ticketEvents: []
                    }
                ]
            },
            updateTicket: {
                id: 0,
                eventDetailId: 0,
                name: null,
                price: 0,
                quantity: 0
            }
        }
    },
    methods: {
        async showModalTicketList(id){
            this.myListTicket = await this.getlistTicketByType(id);
            this.$nextTick(() => {
                    // Using $nextTick to ensure the modal component is updated
                    this.$refs['modal-list-ticket'].show();
                });
        },
        async updateProfile() {
            this.updateEvent.eventName = this.eventInfo.eventName
            this.updateEvent.eventTypeId = this.eventInfo.eventTypeId
            this.updateEvent.supplierId = this.eventInfo.supplierId
            this.updateEvent.eventDescription = this.eventInfo.eventDescription
            this.updateEvent.exchangePolicy = this.eventInfo.exchangePolicy
            this.updateEvent.admissionPolicy = this.eventInfo.admissionPolicy
            this.updateEvent.eventImage = this.eventInfo.eventImage
            this.updateEvent.isExchange = this.eventInfo.isExchange
            this.updateEvent.id = this.eventInfo.id
            try {
                const response = await axios.put('myticket/api/event/update', this.updateEvent, {
                    headers: {
                        'Content-Type': 'application/json',
                        // Add any other headers if needed
                    },
                });

                // Handle the response
                this.notifyVue('Thành công', 'Cập nhật sự kiện thành công', 'top', 'right', 'success')

                // Return the response or do further processing if needed
            } catch (error) {
                // Handle errors
                this.notifyVue('Thất bại', 'Cập nhật sự kiện thất bại', 'top', 'right', 'danger')
                // Throw the error or return an error object if needed
                throw error;
            }
            this.isUpdate = false;
            this.fetchData()
        },
        async addEventDetailFunc() {
            try {
                this.addEventDetail.eventId = this.eventInfo.id;
                const response = await axios.post('myticket/api/event/create-detail', this.addEventDetail, {
                    headers: {
                        'Content-Type': 'application/json',
                        // Add any other headers if needed
                    },
                });
                this.$refs['modal-add-detail'].hide();
                // Handle the response
                this.notifyVue('Thành công', 'Thêm mới sự kiện thành công', 'top', 'right', 'success')

            } catch (error) {
                // Handle errors
                this.$refs['modal-add-detail'].hide();
                this.notifyVue('Thất bại', 'Thêm mới sự kiện thất bại', 'top', 'right', 'danger')
                // Throw the error or return an error object if needed
            }
            this.fetchData()
        },
        async getEventDetail() {
            try {
                const res = await axios.get(
                    "/myticket/api/event/admin/find-by-id", {
                    params: {
                        id: this.$route.query.id
                    }
                })
                return res.data.data
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
        },
        async getlistTicketByType(id) {
            try {
                const res = await axios.get(
                    "/myticket/api/ticket/list/find-all", {
                    params: {
                        id: id
                    }
                })
                return res.data.data
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
        },
        setUpdate() {
            this.isUpdate = true;
        },
        cancelUpdate() {
            this.isUpdate = false;
            this.fetchData()
        },
        async uploadImage() {
            const formData = new FormData();
            formData.append('file', this.seatImage);
            await axios.post('myticket/api/file/upload', formData).then(
                res => this.addEventDetail.eventSeatMapImage = res.data.data
            ).catch(
                err => err
            )
            console.log(this.addEventDetail.eventSeatMapImage)
        },
        async uploadImageUpdateSeat() {
            const formData = new FormData();
            formData.append('file', this.seatImage);
            await axios.post('myticket/api/file/upload', formData).then(
                res => this.updateEventDetail.eventSeatMapImage = res.data.data
            ).catch(
                err => err
            )
            console.log(this.updateEventDetail.eventSeatMapImage)
        },
        async uploadEventImage() {
            const formData = new FormData();
            formData.append('file', this.seatImage);
            await axios.post('myticket/api/file/upload', formData).then(
                res => this.eventInfo.eventImage = res.data.data
            ).catch(
                err => err
            )
            console.log(this.eventInfo.eventImage)
        },
        handleFileChange(event) {
            // Update the 'file' data property when the file input changes
            this.seatImage = event.target.files[0];
        },
        async showModalAddDetail(id) {
            try {
                this.venues = await this.findAllVenue();
                this.$nextTick(() => {
                    // Using $nextTick to ensure the modal component is updated
                    this.$refs['modal-add-detail'].show();
                });
            } catch (error) {
                console.error("Error fetching ticket details:", error);
            }
        },
        async showModalUpdateDetail(item) {
            try {
                this.venues = await this.findAllVenue();
                this.updateEventDetail.id = item.id
                this.updateEventDetail.eventId = item.eventId
                this.updateEventDetail.venueId = item.venueId
                this.updateEventDetail.eventSeatMapImage = item.eventSeatMapImage
                this.updateEventDetail.havingSeatMap = item.havingSeatMap
                this.updateEventDetail.selectSeatType = item.seatSelectType
                this.updateEventDetail.organizationDay = item.organizationDay
                this.updateEventDetail.startSaleTicketDate = item.startSaleTicketDate
                this.updateEventDetail.endSaleTicketDate = item.endSaleTicketDate
                this.$nextTick(() => {
                    // Using $nextTick to ensure the modal component is updated
                    this.$refs['modal-update-detail'].show();
                });
            } catch (error) {

            }
            console.log(this.updateEventDetail)
        },
        async showModalTicket(id, ticketEventId) {
            this.updateTicket.eventDetailId = ticketEventId;
            if (id !== 0) {
                const res = await this.getTicketById(id)
                this.updateTicket.id = res.id,
                    this.updateTicket.name = res.name,
                    this.updateTicket.price = res.price,
                    this.updateTicket.quantity = res.intQuantity
            } else {
                this.updateTicket.id = 0,
                    this.updateTicket.name = null,
                    this.updateTicket.price = 0,
                    this.updateTicket.quantity = 0
            }
            this.$nextTick(() => {
                // Using $nextTick to ensure the modal component is updated
                this.$refs['modal-add-edit-ticket'].show();
            });
        },
        async findAllVenue() {
            try {
                const res = await axios.get(
                    "myticket/api/venue/find",
                    {
                        params: {
                            pageSize: 1000,
                            pageNumber: 1
                        },
                    }
                )
                return res.data.data.items;
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
        },
        async updateEventDetailFunc() {
            try {
                const response = await axios.put('myticket/api/event/detail/update', {
                    venueId: this.updateEventDetail.venueId,
                    organizationDay: moment(this.updateEventDetail.organizationDay, "HH:mm DD/MM/YYYY").format("YYYY-MM-DDTHH:mm:ss.SSS[Z]"),
                    startSaleTicketDate: moment(this.updateEventDetail.startSaleTicketDate, "HH:mm DD/MM/YYYY").format("YYYY-MM-DDTHH:mm:ss.SSS[Z]"),
                    endSaleTicketDate: moment(this.updateEventDetail.endSaleTicketDate, "HH:mm DD/MM/YYYY").format("YYYY-MM-DDTHH:mm:ss.SSS[Z]"),
                    eventSeatMapImage: this.updateEventDetail.eventSeatMapImage,
                    havingSeatMap: this.updateEventDetail.havingSeatMap,
                    selectSeatType: this.updateEventDetail.selectSeatType,
                    id: this.updateEventDetail.id
                }, {
                    headers: {
                        'Content-Type': 'application/json',
                        // Add any other headers if needed
                    },
                });
                if (response.data.code === 200) {
                    this.notifyVue('Thành công', 'Cập nhật sự kiện thành công', 'top', 'right', 'success')
                }
                else {
                    this.notifyVue('Thất bại', 'Cập nhật sự kiện thất bại', 'top', 'right', 'danger')
                }
                // Handle the response

                // Return the response or do further processing if needed
            } catch (error) {
                // Handle errors
                // Throw the error or return an error object if needed
                throw error;
            }
            this.$refs['modal-update-detail'].hide();
            this.fetchData()
        },
        async updateTicketFunc() {
            if (this.updateTicket.id !== 0) {
                const response = await axios.put('myticket/api/user/account/update-by-suplier', {
                    eventDetailId: this.updateTicket.eventDetailId,
                    name: this.updateTicket.name,
                    price: this.updateTicket.price,
                    quantity: this.updateTicket.quantity,
                    id: this.updateTicket.id
                }, {
                    headers: {
                        'Content-Type': 'application/json'
                    },
                })
                if (response.data.code === 200) {
                    this.notifyVue('Thành công', 'Cập nhật vé thành công', 'top', 'right', 'success')
                }
                else {
                    this.notifyVue('Thất bại', 'Cập nhật vé thất bại', 'top', 'right', 'danger')
                }
            } else {
                const response = await axios.post('myticket/api/ticket/create', {
                    eventDetailId: this.updateTicket.eventDetailId,
                    name: this.updateTicket.name,
                    price: this.updateTicket.price,
                    quantity: this.updateTicket.quantity,
                }, {
                    headers: {
                        'Content-Type': 'application/json'
                    },
                })
                if (response.data.code === 200) {
                    this.notifyVue('Thành công', 'Thêm hạng vé thành công', 'top', 'right', 'success')
                }
                else {
                    this.notifyVue('Thất bại', 'Thêm hạng vé thất bại', 'top', 'right', 'danger')
                }
            }
            this.$refs['modal-add-edit-ticket'].hide();
            this.fetchData();
        },
        async getTicketById(id) {
            try {
                const res = await axios.get(
                    "myticket/api/ticket/find-by-id",
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
        async getCategories() {
            try {
                const res = await axios.get(
                    "myticket/api/event-type/find",
                    {
                        params: {
                            pageSize: 10,
                            pageNumber: 1
                        },
                    }
                )
                return res.data.data.items;
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
        },
        async fetchData() {
            try {
                this.eventInfo = await this.getEventDetail()
                this.listType = await this.getCategories();
                this.eventInfo.eventDetails.forEach(element => {
                    element.organizationDay = helpService.formatDate(element.organizationDay)
                    element.endSaleTicketDate = helpService.formatDate(element.endSaleTicketDate)
                    element.startSaleTicketDate = helpService.formatDate(element.startSaleTicketDate)
                    element.ticketEvents.forEach(e => {
                        e.price = helpService.formatCurrency(e.price)
                    });
                    this.eventDetailTable.push({
                        id: element.id,
                        eventName: element.eventName,
                        venueName: element.venueName,
                        organizationDay: element.organizationDay,
                        startSaleTicketDate: element.startSaleTicketDate,
                        endSaleTicketDate: element.endSaleTicketDate,
                        tatus: element.status,
                    })
                });
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
        },
        async updateEventDetailStatus(id, status) {
            let message = "message"; // Initialize with a default value

            switch (status) {
                case 1:
                    message = "Mở sự kiện";
                    break;
                case 2:
                    message = "Mở bán vé";
                    break;
                case 3:
                    message = "Ngừng bán vé";
                    break;
                case 4:
                    message = "Sự kiện diễn ra";
                    break;
                case 5:
                    message = "Hủy sự kiện";
                    break;
                default:
                    break;
            }

            const response = await axios.put('myticket/api/event/detail/update-status', {
                id: id,
                status: status
            }, {
                headers: {
                    'Content-Type': 'application/json'
                },
            });

            if (response.data.code === 200) {
                this.notifyVue('Thành công', `${message} thành công`, 'top', 'right', 'success');
            } else {
                this.notifyVue('Thất bại', `${message} thất bại`, 'top', 'right', 'danger');
            }

            this.fetchData();
        },
        async updateStatusTicket(id, status) {
            let message = "message"; // Initialize with a default value

            switch (status) {
                case 1:
                    message = "Mở bán vé";
                    break;
                case 2:
                    message = "Ngừng bán vé";
                    break;
                default:
                    break;
            }
            const response = await axios.put('myticket/api/ticket/update-status', {
                id: id,
                status: status
            }, {
                headers: {
                    'Content-Type': 'application/json'
                },
            });

            if (response.data.code === 200) {
                this.notifyVue('Thành công', `${message} thành công`, 'top', 'right', 'success');
            } else {
                this.notifyVue('Thất bại', `${message} thất bại`, 'top', 'right', 'danger');
            }

            this.fetchData();
        },
        async updateEventStatus(id, status) {
            let message = "message"; // Initialize with a default value

            switch (status) {
                case 1:
                    message = "Mở sự kiện";
                    break;
                case 5:
                    message = "Hủy sự kiện";
                    break;
                default:
                    break;
            }

            const response = await axios.put('myticket/api/event/update-status', {
                id: id,
                status: status
            }, {
                headers: {
                    'Content-Type': 'application/json'
                },
            });

            if (response.data.code === 200) {
                this.notifyVue('Thành công', `${message} thành công`, 'top', 'right', 'success');
            } else {
                this.notifyVue('Thất bại', `${message} thất bại`, 'top', 'right', 'danger');
            }

            this.fetchData();
        }
    }, mounted() {
        this.fetchData();
        console.log(this.listType)
    },


}

</script>
<style>
#modal-list-ticket___BV_modal_content_{
    height: 500px !important;
}
#modal-list-ticket___BV_modal_body_{
    overflow: scroll !important;
}
.select-form {
    width: 100%;
    height: 40px;
    border: 1px solid #E3E3E3;
    border-radius: 4px;
    padding: 8px 12px;
    color: #565656;
    font-size: 1rem;
}
#modal-list-ticket{
    top: -150px !important;
}
.buttonModal {
    margin: 0 5px !important;
}

.btn-img {
    margin: 0 5px;
}

.modal-header {
    background-color: var(--primary-color-bold) !important;
}

.modal-title {
    font-size: 1.6rem !important;
}

.sub-table {
    width: 100%;
    margin-top: 10px;
    /* Adjust the margin as needed */
}

.sub-table .table-btn {
    font-size: 1rem;
    /* Adjust the font size as needed */
}

.sub-table .btn-fill {
    background-color: #ffc107;
    /* Yellow color, change as needed */
    color: #212529;
    /* Text color, change as needed */
}

.sub-table .float-right {
    float: right;
}

.sub-table .b-card {
    width: 100%;
    margin-top: 10px;
    /* Adjust the margin as needed */
}
</style>