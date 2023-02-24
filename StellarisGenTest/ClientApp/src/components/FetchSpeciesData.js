import React, { Component } from 'react';
//const promise = fetch('cwtest');
export class FetchSpeciesData extends Component {
  static displayName = FetchSpeciesData.name;

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true };
  }

  componentDidMount() {
    this.populateSpeciesData();
  }

  static renderSpeciesTable(forecasts) {
    return (
      <table className="table table-striped" aria-labelledby="tableLabel">
        <thead>
          <tr>
            <th>Name</th>
            <th>Archetype</th>
            <th>Playable</th>
          </tr>
        </thead>
        <tbody>
          {forecasts.map(forecast =>
            <tr key={forecast.name}>
              <td>{forecast.name}</td>
              <td>{forecast.archetype}</td>
              <td>{forecast.playable}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchSpeciesData.renderSpeciesTable(this.state.forecasts);

    return (
      <div>
        <h1 id="tableLabel">Species Data</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateSpeciesData() {
    const response = await fetch('cwspeciesclass');
    console.log(response);
    const data = await response.json();
    console.log("Test");
    this.setState({ forecasts: data, loading: false });
  }
}
