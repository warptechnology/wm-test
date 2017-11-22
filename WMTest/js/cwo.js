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

    let Table = [
        document.querySelector('.TaskTable1'),
        document.querySelector('.TaskTable2'),
        document.querySelector('.TaskTable3')
    ];

    button1.onclick = () => {
        const value = taskOneValueInput.value;
        get(REST_URL_ONE, value).then(result => {
            taskOneResult.innerHTML = result;
        });
        get_Table(1);
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

        get_Table(2);
    }

    button3.onclick = () => {
        const source = taskThreeID1Input.value;
        const destination = taskThreeID2Input.value;
        const value = taskThreeValueInput.value;
        let request = '?';
        request += $.param({ id1: source, id2: destination, amount: value });

        get(REST_URL_THREE, request).then(result => {
            taskThreeResult.innerHTML = result;
        });

        get_Table(3);
    }

    get_Table = (TaskNumber) => {
        if (TaskNumber != 1 && TaskNumber != 2 && TaskNumber != 3)
            return;
        let result = get("Table", TaskNumber).then(result => {
            let arr = JSON.parse(result);
            if (Array.isArray(arr)) {
                $(Table[TaskNumber - 1]).empty();
                for (var i = 0; i < arr.length; i++) {
                    $(Table[TaskNumber - 1]).append(`<tr><th scope="row">${arr[i].Item1}</th><td>${arr[i].Item2}</td></tr>`);
                }
            }
        });
    };
    get_Table(1);
    get_Table(2);
    get_Table(3);
});

