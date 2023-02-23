import React, { Component } from 'react';

export class FetchCwData extends Component {
  static displayName = FetchCwData.name;

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true };
  }

  componentDidMount() {
    this.populateCwData();
  }

  static renderCwTable(forecasts) {
    return (
      <table className="table table-striped" aria-labelledby="tableLabel">
        <thead>
          <tr>
            <th>Name</th>
            <th>Children</th>
          </tr>
        </thead>
        <tbody>
          {forecasts.map(forecast =>
            <tr key={forecast.name}>
              <td>{forecast.name}</td>
              <td>{forecast.test}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchCwData.renderCwTable(this.state.forecasts);

    return (
      <div>
        <h1 id="tableLabel">CW Data</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateCwData() {
    const response = await fetch('cwtest');
    console.log(response);
    const data = await response.json();
    console.log("Test");
    this.setState({ forecasts: data, loading: false });
  }
}
