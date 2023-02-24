import React, { Component } from 'react';
//const promise = fetch('cwtest');
export class FetchEthicsData extends Component {
    static displayName = FetchEthicsData.name;

    constructor(props) {
        super(props);
        this.state = { forecasts: [], loading: true };
    }

    componentDidMount() {
        this.populateEthicsData();
    }

    static renderEthicsTable(forecasts) {
        return (
            <table className="table table-striped" aria-labelledby="tableLabel">
                <thead>
                <tr>
                    <th>Ethic</th>
                    <th>Cost</th>
                    <th>category</th>
                    <th>category Value</th>
                </tr>
                </thead>
                <tbody>
                {forecasts.map(forecast =>
                    <tr key={forecast.name}>
                        <td>{forecast.cost}</td>
                        <td>{forecast.category}</td>
                        <td>{forecast.categorycost}</td>
                    </tr>
                )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchEthicsData.renderEthicsTable(this.state.forecasts);

        return (
            <div>
                <h1 id="tableLabel">Ethics Data</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        );
    }

    async populateEthicsData() {
        const response = await fetch('ethicsclass');
        console.log(response);
        const data = await response.json();
        console.log("Test");
        this.setState({ forecasts: data, loading: false });
    }
}
