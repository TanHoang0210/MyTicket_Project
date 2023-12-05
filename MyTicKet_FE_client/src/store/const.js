// constants.js
export const OrderStatuses = Object.freeze({
  INIT: 1,
  READY_TO_PAY: 2,
  PAYING: 3,
  CANCEL: 4,
  SUCCESS: 5
});

export const TicketStatuses = Object.freeze({
  ACTIVE: 1,
  DEACIVE: 2,
  SOLDOUT: 3
});

export const MyAsset = Object.freeze({
  SOLD_OUT_IMG: 'api/file/get?folder=asset&file=soldout.webp'
});