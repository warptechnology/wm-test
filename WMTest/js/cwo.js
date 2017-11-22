const SERVER_ADDRESS = window.location.href;
const REST = "api/Task/";

get = (REST_URL, payload) => {
    payload = payload ? `/${payload}` : ``;
    const REQUEST_URL = `${SERVER_ADDRESS}${REST}${REST_URL}${payload}`;
    return $.get(REQUEST_URL);
};

post = (REST_URL, body, callback, dataType) => {
    return $.post(`${SERVER_ADDRESS}${REST}${REST_URL}`, body, callback, dataType);
};

$().ready(() => {
    const REST_URL_ONE = "GetOrAdd";
    const REST_URL_TWO = "AddOrUpdate";
    const REST_URL_THREE = "TransferMoney";
    const button1 = document.querySelector('.getOrAdd');

    const taskOneValueInput = document.querySelector('.taskOneValueInput');
    const taskOneResult = document.querySelector('.taskOneResult');

    const taskTwoIDInput = document.querySelector('.taskTwoIDInput');
    const taskTwoValueInput = document.querySelector('.taskTwoValueInput');
    const taskTwoResult = document.querySelector('.taskTwoResult');
    const button2 = document.querySelector('.addOrUpdate');

    const taskThreeValueInput = document.querySelector('.taskThreeValueInput');
    const taskThreeID1Input = document.querySelector('.taskThreeID1Input');
    const taskThreeID2Input = document.querySelector('.taskThreeID2Input');
    const taskThreeResult = document.querySelector('.taskThreeResult');
    const button3 = document.querySelector('.transfer');

    button1.onclick = () => {
        const value = taskOneValueInput.value;
        get(REST_URL_ONE, value).then(result => {
            taskOneResult.innerHTML = result;
        });
    }

    button2.onclick = () => {
        const id = taskTwoIDInput.value;
        const value = taskTwoValueInput.value;
        const request = { ID: id, Value: value };
        const callback = result => {
            taskTwoResult.innerHTML = result;
        }

        const dataType = 'json';
        post(REST_URL_TWO, JSON.stringify(request), callback, dataType);
    }

    button3.onclick = () => {
        const source = taskThreeID1Input.value;
        const destination = taskThreeID2Input.value;
        const value = taskThreeValueInput.value;
        let request = { id1: source, id2: destination, amount: value };

        get(REST_URL_THREE, request).then(result => {
            taskThreeResult.innerHTML = result;
        });
    }
});