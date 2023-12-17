import Vue from 'vue'
import Vuex from 'vuex'
Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    currentUser: null,
    accessToken: null,
    refreshToken: null,
    tokenExpiration: null,
    orderFormData: null,
    countdownTime: 300,
    createEvent: {
      eventName: "string",
      eventTypeId: 0,
      supplierId: 0,
      eventDescription: "string",
      exchangePolicy: "string",
      admissionPolicy: "string",
      eventImage: "string",
      isExchange: true,
      eventDetails: [
        {
          venueId: 0,
          organizationDay: "2023-12-12T14:54:45.016Z",
          startSaleTicketDate: "2023-12-12T14:54:45.016Z",
          endSaleTicketDate: "2023-12-12T14:54:45.016Z",
          eventSeatMapImage: "string",
          havingSeatMap: true,
          selectSeatType: 0,
          ticketEvents: [
            {
              name: "string",
              price: 0,
              quantity: 0
            }
          ]
        }
      ]
    }
  },
  getters: {
    currentUser: state => state.currentUser,
    accessToken: state => state.accessToken,
    refreshToken: state => state.refreshToken,
    tokenExpiration: state => state.tokenExpiration,
    orderFormData: state => state.orderFormData,
    countdownTime: state => state.countdownTime,
    createEvent: state => state.createEvent
  },
  mutations: {
    setOrderData(state, orderFormData) {
      state.orderFormData = orderFormData;
    },
    setCoutDownOrder(state, countdownTime) {
      state.countdownTime = countdownTime;
    },
    setCurrentUser(state, user) {
      state.currentUser = user;
    },
    setTokens(state, { accessToken, refreshToken, tokenExpiration }) {
      state.accessToken = accessToken;
      state.refreshToken = refreshToken;
      state.tokenExpiration = tokenExpiration;
    },
    setEventToCreate(state, createEvent) {
      state.createEvent = createEvent;
    },
    logout(state) {
      sessionStorage.removeItem('accessToken'),
        sessionStorage.removeItem('refreshToken'),
        sessionStorage.removeItem('currentUser'),
        sessionStorage.removeItem('tokenExpiration'),
        state.accessToken = null;
      state.refreshToken = null;
      state.tokenExpiration = null;
      state.currentUser = null;
    }
  },
  actions: {
    logout({ commit }) {
      commit('logout');
    },
  },
  modules: {
  }
})
