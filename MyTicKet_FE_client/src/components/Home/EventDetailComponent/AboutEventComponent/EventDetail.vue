<template>
    <section id="eventDetail" class="bg-grey box-shadow pb-4 pt-3 section-content">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12 mgt-16 pt-3 pb-3 pb-sm-5">
                    <h1 class="black-heading">EVENT DETAILS</h1>
                    <div class="row">
                        <div id="activityContent" class="col-lg-9 col-md-9 col-sm-12 col-xs-12 col-12">
                            <div style="justify-content: center;letter-spacing: 1px; font-size: 18px;">
                                <p>Euphony: Harmony of Sounds là sự hợp tác giữa cộng đồng người Hoa tại Republic
                                    Polytechnic
                                    Concerto và đàn dây
                                    Buổi hòa nhạc.</p>
                                <p>Buổi hòa nhạc đôi này sẽ mang lại sự hài hòa của các bản giao hưởng khác nhau của
                                    truyền thống và cổ điển
                                    âm nhạc, nhằm mục đích thể hiện sự độc đáo của từng nhạc cụ và cá nhân. Cái này
                                    hiệu suất cũng sẽ
                                    có các nhạc sĩ từ The Purple Symphony và Inclusive Arts Movement (I.AM).</p>
                                <p>Bằng cách tôn vinh sự đa dạng và hòa hợp thông qua niềm đam mê âm nhạc chung của họ, điều
                                    này
                                    hiệu suất đó
                                    vượt qua rào cản sẽ khiến khán giả được truyền cảm hứng.</p>
                                <p>
                                    <i>
                                        Euphony: Harmony of Sounds là một trong những điểm nổi bật của lễ hội trong khuôn
                                        khổ Republic
                                        Suy ngẫm của Bách khoa
                                        Lễ hội nghệ thuật cộng đồng.
                                    </i>
                                </p>
                            </div>
                        </div>
                        <div id="sponsorSidebar" class="col-md-3 col-sm-12 col-xs-12 sponsor">
                            <div class="row row-no-gutters">
                                <div id="left-sidebar-top"
                                    class="col-md-12 col-sm-12 col-xs-12 d-none d-sm-block text-center pb-3">
                                    <ul class="additional-button list-unstyled p-0">
                                        <li>
                                            <button v-on:click="modalShow = true" id="md-viewMap"
                                                class="btn btn-outline-primary w-90 mb-3 action-btn viewmap-btn">Xem vị trí
                                                chỗ ngồi</button>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="disclaimerBox-top" class="row mgt-24">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                    <div class="alert alert-notify-yellow">
                        Vui lòng đọc và đồng ý với tất cả các điều khoản và điều kiện trước khi đặt hàng. Việc mua hoặc hoàn
                        tiền được coi là hoàn toàn đồng ý với tất cả các quy tắc trên.
                    </div>
                </div>
            </div>
        </div>
    <div v-show="modalShow">
    <div class="modal-ready" v-on:click="modalShow =false">

    </div>
    <div class="modal-seatmap"  style="overflow: hidden auto;" tabindex="-1">
        <div class="modal-seatmap-inner">
            <div class="map-content">
            <div class="map-figure">
                <button v-on:click="modalShow =false" type="button" class="map-close">
                    <b-icon icon="x-lg"></b-icon>
                </button>
                <figure v-on:click="zoomMap()" style="cursor: zoom-in;">
                        <img class="mfp-img"
                            src="https://static.ticketmaster.sg/images/activity/field/23_euphony2023_dd028f21b48dced710f997c10541d4c0.jpg"
                            style="max-height: 640px;">
                    </figure>
            </div>
        </div>
        </div>
    </div>
</div>
    </section>
</template>
<script>
export default {
    data() {
        return {
            modalShow: false
        }
    }
}
</script>
<style>
#sponsorSidebar {
    max-width: 100%;
    padding-right: calc(var(--bs-gutter-x) * 0.5);
    padding-left: calc(var(--bs-gutter-x) * 0.5);
    margin-top: var(--bs-gutter-y);
}

p {
    margin-top: 0;
    margin-bottom: 1rem;
}

.alert {
    padding: 16px;
    border: 1px solid #F2bd2a !important;
    border-radius: 0.25rem;
    background-color: #f9f2de;
    color: #262626 !important;
}

.viewmap-btn {
    color: var(--primary-color-bold) !important;
    border-color: var(--primary-color-bold) !important;
    width: 90%;
    font-weight: 500 !important;
    transition: all 0.2s ease !important;
}

.viewmap-btn:hover {
    background-color: var(--primary-color-bold) !important;
    color: #fff !important;
}
.modal-ready{
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    z-index: 1042;
    overflow: hidden;
    position: fixed;
    background: #0b0b0b;
    opacity: .8;
}
.modal-seatmap{
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    z-index: 1043;
    position: fixed;
    outline: none !important;
    -webkit-backface-visibility: hidden;
}
.modal-seatmap-inner {
    text-align: center;
    position: absolute;
    width: 100%;
    height: 100%;
    left: 0;
    top: 0;
    padding: 0 8px;
    box-sizing: border-box;
}
.map-figure{
    cursor: pointer;
}
.map-close{
    color: #FFF;
    right: -6px;
    text-align: right;
    padding-right: 6px;
    width: 100%;
    background-color: transparent;
    border: none !important;
}
.map-content{
    position: relative;
    display: inline-block;
    vertical-align: middle;
    margin: 0 auto;
    text-align: left;
    z-index: 1045;
}
.mfp-img{
    width: auto;
    max-width: 100%;
    height: auto;
    display: block;
    line-height: 0;
    box-sizing: border-box;
    padding: 40px 0 40px;
    margin: 0 auto;
}
</style> 